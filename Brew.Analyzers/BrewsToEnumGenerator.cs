using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brew.Generators;

[Generator]
public sealed class BrewsToEnumGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDecls = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax cls && cls.BaseList is not null,
            static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);

        var brewClassNames = classDecls
            .Combine(context.CompilationProvider)
            .Select(static (pair, _) =>
            {
                var (cls, compilation) = pair;
                var model = compilation.GetSemanticModel(cls.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(cls) as INamedTypeSymbol;
                if (symbol is null)
                    return null;
                var implementsIBrew = symbol.AllInterfaces.Any(i => string.Equals(i.Name, "IBrew", StringComparison.Ordinal));
                return implementsIBrew ? symbol.Name : null;
            })
            .Where(static name => name is not null)!
            .Select(static (name, _) => name!);

        context.RegisterSourceOutput(brewClassNames.Collect(), static (ctx, names) =>
        {
            var unique = names.Distinct().OrderBy(n => n).ToList();
            var enumList = new List<EnumMemberDeclarationSyntax>();
            foreach (var n in unique)
            {
                enumList.Add(EnumMemberDeclaration(Identifier(n)));
            }

            var code = CompilationUnit()
                .WithUsings(new SyntaxList<UsingDirectiveSyntax>(new[] { UsingDirective(IdentifierName(nameof(System))) }))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        FileScopedNamespaceDeclaration(IdentifierName(nameof(Brew)))
                            .WithMembers(
                                SingletonList<MemberDeclarationSyntax>(
                                    EnumDeclaration("Brews")
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                        .WithMembers(SeparatedList(enumList))))))
                .NormalizeWhitespace();

            ctx.AddSource("Brews.g.cs", code.ToFullString());
        });
    }
}