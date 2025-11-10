using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brew.Features.Serialization.Avro.SourceGenerator;

[Generator]
public sealed class AvroModelsToRepositoriesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDecls = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax cls && cls.BaseList is not null,
            static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);

        var avroModelNames = classDecls
            .Combine(context.CompilationProvider)
            .Select(static (pair, _) =>
            {
                var (cls, compilation) = pair;
                var model = compilation.GetSemanticModel(cls.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(cls) as INamedTypeSymbol;
                if (symbol is null)
                    return null;
                var implements = symbol.AllInterfaces.Any(i => string.Equals(i.Name, "ISpecificRecord", StringComparison.Ordinal));
                return implements ? symbol.Name : null;
            })
            .Where(static name => name is not null)!
            .Select(static (name, _) => name!);

        context.RegisterSourceOutput(avroModelNames.Collect(), static (ctx, names) =>
        {
            var unique = names.Distinct().OrderBy(n => n).ToList();
            var modelRepositories = new List<MemberDeclarationSyntax>();
            foreach (var n in unique)
            {
                modelRepositories.Add(
                    FieldDeclaration(
                        VariableDeclaration(
                                GenericName(Identifier("GenericRepository"))
                                    .WithTypeArgumentList(
                                        TypeArgumentList(
                                            SingletonSeparatedList<TypeSyntax>(IdentifierName(n)))))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(Identifier(n + "Repository"))
                                        .WithInitializer(EqualsValueClause(ImplicitObjectCreationExpression()))))));
            }

            var code = CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        FileScopedNamespaceDeclaration(
                                QualifiedName(IdentifierName("Brew"), IdentifierName("Avro")))
                            .WithUsings(
                                SingletonList(
                                    UsingDirective(QualifiedName(IdentifierName("Brew"), IdentifierName("Models")))))
                            .WithMembers(
                                SingletonList<MemberDeclarationSyntax>(
                                    ClassDeclaration("Repositories")
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                        .WithMembers(List(modelRepositories))))))
                .NormalizeWhitespace();

            ctx.AddSource("ModelsApi.g.cs", code.ToFullString());
        });
    }
}