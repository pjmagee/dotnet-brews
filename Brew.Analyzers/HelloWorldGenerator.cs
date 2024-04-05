using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brew.Generators;

[Generator]

public class HelloWorldGenerator : ISourceGenerator
{
    private SyntaxList<AttributeListSyntax> Generated => SingletonList(
        AttributeList(
            SingletonSeparatedList(
                Attribute(IdentifierName("GeneratedCode")).WithArgumentList(AttributeArgumentList(SeparatedList<AttributeArgumentSyntax>(new SyntaxNodeOrToken[]
                    {
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(nameof(HelloWorldGenerator)))), Token(SyntaxKind.CommaToken),
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("1.0.0")))
                    })
                ))
            )));


    public void Initialize(GeneratorInitializationContext context)
    {

    }

    public void Execute(GeneratorExecutionContext context)
    {
        var code =

            CompilationUnit()
                .WithUsings
                (
                    new SyntaxList<UsingDirectiveSyntax>(new[] { UsingDirective(IdentifierName(nameof(System))), UsingDirective(IdentifierName("System.CodeDom.Compiler")) })
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
                                    ClassDeclaration("Hello")
                                        //.WithAttributeLists(Generated)
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword))
                                )
                                .WithMembers
                                (
                                    SingletonList<MemberDeclarationSyntax>
                                    (
                                        MethodDeclaration
                                            (
                                                PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("World"))
                                                    .WithBody(
                                                        Block
                                                        (
                                                            SingletonList<StatementSyntax>(ExpressionStatement(InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("Console"), IdentifierName("WriteLine"))
                                                        )
                                                        .WithArgumentList
                                                        (
                                                            ArgumentList
                                                            (
                                                                SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("Hello World"))))
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                        ).WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                    )
                                )
                            )
                        )
                    )
                )
                .NormalizeWhitespace();

        context.AddSource("Hello.g.cs", code.NormalizeWhitespace().ToFullString());
    }
}