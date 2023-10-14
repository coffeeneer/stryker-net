using System;
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
        [InlineData("int[] test = new [] { -1, 3 }", "int")]
        [InlineData("uint[] test = new [] { 1, 3 }", "uint")]
        [InlineData("long[] test = new [] { -1, 3 }", "long")]
        [InlineData("ulong[] test = new [] { 1, 3 }", "ulong")]
        [InlineData("double[] test = new [] { 1.2, 3.3 }", "double")]
        [InlineData("float[] test = new [] { 1.1f, 3.2f }", "float")]
        [InlineData("decimal[] test = new [] { 1.1M, 3.2M }", "decimal")]
        [InlineData("char[] test = new [] { 'c' }", "char")]
        [InlineData("string[] test = new [] { \"something\" }", "string")]
        [InlineData("Person[] test = new [] { new(), new() }", "Person")]
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
        [InlineData("var test = new [] { -1, 3 }", "int")]
        [InlineData("var test = new [] { -1, 3 }", "long")]
        [InlineData("var test = new [] { 1.2d, 3.3d }", "double")]
        [InlineData("var test = new [] { 1.1f, 3.2f }", "float")]
        [InlineData("var test = new [] { 'c' }", "char")]
        [InlineData("var test = new [] { \"something\" }", "string")]
        [InlineData("var test = new [] { new Person(), new Person() }", "Person")]
        public void ShouldRemoveValuesFromImplicitArrayCreationAssignment(string assignmentStatement, string type)
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


        [Fact]
        public void TestTest()
        {
            var expressionSyntax = SyntaxFactory.ParseExpression("new [] { 1, 3.2, 3.2f, 2.3M, 'c', \"test\", new Person(), someVariable, SomeMethod(), someVar.SomeMethod(), someVar.Property }") as ImplicitArrayCreationExpressionSyntax;

            int[] test = default;

            foreach (var ie in expressionSyntax.Initializer.Expressions) 
            {
                var kind = ie.Kind();
                Console.WriteLine(kind);
            }

            var target = new ImplicitArrayCreationMutator();

            var result = target.ApplyMutations(expressionSyntax);

            result.ShouldBeEmpty();
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
