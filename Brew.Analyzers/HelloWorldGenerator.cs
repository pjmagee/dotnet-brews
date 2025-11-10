using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brew.Generators;

[Generator]
public sealed class HelloWorldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
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
                                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
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
                                                                SingletonList<StatementSyntax>(
                                                                    ExpressionStatement(
                                                                        InvocationExpression(
                                                                            MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                IdentifierName("Console"),
                                                                                IdentifierName("WriteLine")))
                                                                        .WithArgumentList(
                                                                            ArgumentList(
                                                                                SingletonSeparatedList(
                                                                                    Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("Hello World"))))))))))
                                                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                                )
                                            )
                                    )
                                )
                        )
                    )
                    .NormalizeWhitespace();

            ctx.AddSource("Hello.g.cs", code.ToFullString());
        });
    }
}