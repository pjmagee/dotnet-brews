using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brew.Generators;

[Generator]
public class BrewsToEnumGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {

    }

    public void Execute(GeneratorExecutionContext context)
    {
        var enumList = new List<EnumMemberDeclarationSyntax>();

        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var classes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            var classesImplementingIBrew = classes.Where(x => x.BaseList != null && x.BaseList.DescendantTokens().Any(t => t.Text.Equals("IBrew")));
            
            foreach (var @class in classesImplementingIBrew)
            {
                enumList.Add(EnumMemberDeclaration(Identifier(@class.Identifier.Text)));
            }
        }

        var code = CompilationUnit()
            .WithUsings
            (
                new SyntaxList<UsingDirectiveSyntax>(new[]
                {
                    UsingDirective(IdentifierName(nameof(System)))
                })
            )
            .WithMembers
            (
                SingletonList<MemberDeclarationSyntax>
                (
                    FileScopedNamespaceDeclaration(IdentifierName(nameof(Brew)))
                        .WithMembers
                        (
                            SingletonList<MemberDeclarationSyntax>
                            (
                                EnumDeclaration("Brews")
                                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                    .WithMembers(SeparatedList(enumList))
                            )
                        )
                )
            ).NormalizeWhitespace();


        context.AddSource("Brews.g.cs", code.ToFullString());
    }
}