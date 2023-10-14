using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stryker.Core.Mutants;
using System.Collections.Generic;

namespace Stryker.Core.Mutators
{
    public class ImplicitArrayCreationMutator : MutatorBase<ExpressionSyntax>, IMutator
    {
        public override MutationLevel MutationLevel => MutationLevel.Standard;

        public override IEnumerable<Mutation> ApplyMutations(ExpressionSyntax node)
        {
            if (node is ImplicitArrayCreationExpressionSyntax arrayCreationNode && arrayCreationNode.Initializer?.Expressions != null && arrayCreationNode.Initializer.Expressions.Count > 0)
            {
                var mutation = CreateMutation(arrayCreationNode);
                if (mutation != null)
                {
                    yield return mutation;
                }
            }
        }

        private static Mutation CreateMutation(ImplicitArrayCreationExpressionSyntax node)
        {
            var arrayType = GetArrayType(node);
            if(arrayType is null) {
                return null;
            }

            var arrayCreation = SyntaxFactory.ArrayCreationExpression(SyntaxFactory
                .ArrayType(arrayType)
                .WithRankSpecifiers(SyntaxFactory.SingletonList(
                    SyntaxFactory.ArrayRankSpecifier(
                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                            SyntaxFactory.OmittedArraySizeExpression())))))
                    .WithInitializer(SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression));

            return new Mutation
            {
                OriginalNode = node,
                ReplacementNode = arrayCreation,
                DisplayName = "Implicit array initializer mutation",
                Type = Mutator.Initializer
            };
        }

        private static TypeSyntax GetArrayType(ImplicitArrayCreationExpressionSyntax node)
        {
            var t = new [] { 1, 2, 3 };
            
            // Try to get type from variable declaration
            var declaration = node.FirstAncestorOrSelf<VariableDeclarationSyntax>();
            if (declaration is VariableDeclarationSyntax variableDeclaration)
            {
                if (variableDeclaration.Type is ArrayTypeSyntax arrayType)
                {
                    return arrayType.ElementType;
                }

                // TODO: Maybe get type from collection or span generic etc.
            }

            // Try to get type from initializer
            foreach (var expression in node.Initializer.Expressions)
	        {
                expression.Kind();
	        }
           

            return null;
        }
    }
}
