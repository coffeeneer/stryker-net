using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stryker.Core.Mutants;
using System;
using System.Collections.Generic;

namespace Stryker.Core.Mutators
{
    public class ImplicitArrayCreationMutator : MutatorBase<ExpressionSyntax>, IMutator
    {
        public override MutationLevel MutationLevel => MutationLevel.Standard;

        public override IEnumerable<Mutation> ApplyMutations(ExpressionSyntax node)
        {
            if (node is ImplicitStackAllocArrayCreationExpressionSyntax stackAllocArray && stackAllocArray.Initializer?.Expressions != null && stackAllocArray.Initializer.Expressions.Count > 0)
            {
                var mutation = CreateMutation(stackAllocArray);
                if (mutation != null)
                {
                    yield return mutation;
                }
            }

            if (node is ImplicitArrayCreationExpressionSyntax arrayCreationNode && arrayCreationNode.Initializer?.Expressions != null && arrayCreationNode.Initializer.Expressions.Count > 0)
            {
                var mutation = CreateMutation(arrayCreationNode);
                if (mutation != null)
                {
                    yield return mutation;
                }
            }
        }

        private Mutation CreateMutation(ExpressionSyntax node)
        {
            var arrayType = GetArrayType(node);
            if(arrayType is null) {
                return null;
            }

            var arrayTypeSyntax = SyntaxFactory
                .ArrayType(arrayType)
                .WithRankSpecifiers(SyntaxFactory.SingletonList(
                    SyntaxFactory.ArrayRankSpecifier(
                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                            SyntaxFactory.OmittedArraySizeExpression()))));

            ExpressionSyntax arrayCreation = node is ImplicitStackAllocArrayCreationExpressionSyntax ?
                SyntaxFactory.StackAllocArrayCreationExpression(arrayTypeSyntax)
                    .WithInitializer(SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression)) :
                SyntaxFactory.ArrayCreationExpression(arrayTypeSyntax)
                    .WithInitializer(SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression));

            return new Mutation
            {
                OriginalNode = node,
                ReplacementNode = arrayCreation,
                DisplayName = "Implicit array initializer mutation",
                Type = Mutator.Initializer
            };
        }

        private static TypeSyntax GetArrayType(ExpressionSyntax node)
        {
            // Try to get type from variable declaration
            var declaration = node.FirstAncestorOrSelf<VariableDeclarationSyntax>();
            if (declaration is VariableDeclarationSyntax variableDeclaration &&
                variableDeclaration.Type is ArrayTypeSyntax arrayType)
            {
                return arrayType.ElementType;
            }

            return null;
        }
    }
}
