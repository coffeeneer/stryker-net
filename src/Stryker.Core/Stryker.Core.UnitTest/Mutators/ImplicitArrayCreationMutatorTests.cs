using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shouldly;
using Stryker.Core.Mutators;
using Xunit;

namespace Stryker.Core.UnitTest.Mutators
{
    public class ImplicitArrayCreationMutatorTests : TestBase
    {
        [Fact]
        public void ShouldBeMutationLevelStandard()
        {
            var target = new ImplicitArrayCreationMutator();
            target.MutationLevel.ShouldBe(MutationLevel.Standard);
        }

        [Theory]
        [InlineData("int[] test = new [] { 1, 3 }", "int")]
        [InlineData("double[] test = new [] { 1.2, 3.3 }", "double")]
        [InlineData("float[] test = new [] { 1.1, 3.2 }", "float")]
        [InlineData("Person[] test = new [] { new(), new() }", "Person")]
        [InlineData("string[] test = new [] { \"something\" }", "string")]
        public void ShouldRemoveValuesFromImplicitArrayCreationWithTypedAssignment(string assignmentStatement, string type)
        {
            var expressionSyntax = SyntaxFactory.ParseStatement(assignmentStatement);
            var arrayCreationNode = expressionSyntax
                .DescendantNodes()
                .OfType<ImplicitArrayCreationExpressionSyntax>()
                .First() as ExpressionSyntax;

            var target = new ImplicitArrayCreationMutator();

            var result = target.ApplyMutations(arrayCreationNode);

            var mutation = result.ShouldHaveSingleItem();
            mutation.DisplayName.ShouldBe("Implicit array initializer mutation");

            var replacement = mutation.ReplacementNode.ShouldBeOfType<ArrayCreationExpressionSyntax>();
            replacement.Type.ElementType.ToString().ShouldBe(type);
            replacement.Initializer.Expressions.ShouldBeEmpty();
        }

        [Theory]
        [InlineData("int[] test = stackalloc [] { 1, 3 }", "int")]
        [InlineData("double[] test = stackalloc [] { 1.2, 3.3 }", "double")]
        [InlineData("float[] test = stackalloc [] { 1.1, 3.2 }", "float")]
        [InlineData("Person[] test = stackalloc [] { new(), new() }", "Person")]
        [InlineData("string[] test = stackalloc [] { \"something\" }", "string")]
        public void ShouldRemoveValuesFromImplicitStackAllocArrayCreationWithTypedAssignment(string assignmentStatement, string type)
        {
            var expressionSyntax = SyntaxFactory.ParseStatement(assignmentStatement);
            var arrayCreationNode = expressionSyntax
                .DescendantNodes()
                .OfType<ImplicitStackAllocArrayCreationExpressionSyntax>()
                .First() as ExpressionSyntax;

            var target = new ImplicitArrayCreationMutator();

            var result = target.ApplyMutations(arrayCreationNode);

            var mutation = result.ShouldHaveSingleItem();
            mutation.DisplayName.ShouldBe("Implicit array initializer mutation");

            var replacement = mutation.ReplacementNode.ShouldBeOfType<StackAllocArrayCreationExpressionSyntax>();
            replacement.Type.ToString().ShouldBe(type);
            replacement.Initializer.Expressions.ShouldBeEmpty();
        }

        [Fact]
        public void ShouldNotRemoveValuesFromArrayCreation()
        {
            var expressionSyntax = SyntaxFactory.ParseExpression("new int[] { 1, 3 }") as ArrayCreationExpressionSyntax;

            var target = new ImplicitArrayCreationMutator();

            var result = target.ApplyMutations(expressionSyntax);

            result.ShouldBeEmpty();
        }
    }
}
