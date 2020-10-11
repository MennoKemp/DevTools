using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevTools.Analysis
{
    public static class Extensions
    {
        public static T FindAttribute<T>(this XmlElementStartTagSyntax startTag) where T : XmlAttributeSyntax
        {
            return startTag?.Attributes.OfType<T>().FirstOrDefault();
        }

        public static bool EqualsType(this TypeInfo typeInfo, Type type)
        {
            return typeInfo.Type.ToString().Equals(type.FullName);
        }

        public static bool IsAnyOf<T>(this T obj, params T[] others)
        {
            return others.Contains(obj);
        }

        public static IEnumerable<SyntaxNode> GetExceptionSources(this SyntaxNode syntaxNode)
        {
            SyntaxNode expression = syntaxNode switch
            {
                ThrowStatementSyntax throwStatement => throwStatement.Expression,
                ThrowExpressionSyntax throwExpression => throwExpression.Expression,
                _ => null
            };

            switch (expression)
            {
                case ObjectCreationExpressionSyntax _:
                {
                    yield return expression;
                    break;
                }
                case ConditionalExpressionSyntax conditionalExpression:
                {
                    foreach (ExpressionSyntax resultExpression in new[] { conditionalExpression.WhenTrue, conditionalExpression.WhenFalse })
                    {
                        List<SyntaxNode> descendantNodes = resultExpression.DescendantNodesAndSelf().ToList();

                        if (descendantNodes.OfType<ObjectCreationExpressionSyntax>().FirstOrDefault() is ObjectCreationExpressionSyntax exceptionCreation)
                            yield return exceptionCreation;
                        else if (descendantNodes.OfType<IdentifierNameSyntax>().LastOrDefault() is IdentifierNameSyntax identifier)
                            yield return identifier;
                    }
                    
                    break;
                }
                default:
                {
                    if (expression?.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>().LastOrDefault() is IdentifierNameSyntax identifier)
                        yield return identifier;

                    break;
                }
            }
        }
    }
}
