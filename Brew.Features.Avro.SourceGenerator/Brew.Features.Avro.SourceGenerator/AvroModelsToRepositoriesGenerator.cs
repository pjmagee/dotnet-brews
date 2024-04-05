using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brew.Features.Avro.SourceGenerator;

[Generator]
public class AvroModelsToRepositoriesGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var modelRepositories = new List<MemberDeclarationSyntax>();

        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var classes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            var avroModels = classes.Where(x => x.BaseList != null && x.BaseList.DescendantTokens().Any(t => t.Text.Equals("ISpecificRecord")));

            foreach (var @class in avroModels)
            {
                modelRepositories.Add(FieldDeclaration(
                    VariableDeclaration(
                            GenericName(
                                    Identifier("GenericRepository"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(
                                            IdentifierName(@class.Identifier.Text)))))
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                        Identifier(@class.Identifier.Text + "Repository"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ImplicitObjectCreationExpression()))))));
            }
        }

        var code = CompilationUnit()
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    FileScopedNamespaceDeclaration(
                            QualifiedName(
                                IdentifierName("Brew"),
                                IdentifierName("Avro")))
                        .WithUsings(
                            SingletonList(
                                UsingDirective(
                                    QualifiedName(
                                        IdentifierName("Brew"),
                                        IdentifierName("Models")))))

                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration("Repositories")
                                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                    .WithMembers(List(modelRepositories))))));


        context.AddSource("ModelsApi.g.cs", code.NormalizeWhitespace().ToFullString());
    }
}