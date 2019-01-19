using System.Linq.Expressions;
using Demon.Fody.PointcutExpression.Token;

namespace Demon.Fody.PointcutExpression
{
    public class ExpressionVisitor : ITokenVisitor<Expression>
    {
        public Expression Visit(AndAlsoToken andAlso)
        {
            throw new System.NotImplementedException();
        }

        public Expression Visit(ExecutionToken execution)
        {
            throw new System.NotImplementedException();
        }

        public Expression Visit(NotToken not)
        {
            throw new System.NotImplementedException();
        }

        public Expression Visit(OrElseToken orElse)
        {
            throw new System.NotImplementedException();
        }

        public Expression Visit(PointcutToken pointcut)
        {
            throw new System.NotImplementedException();
        }

        public Expression Visit(WithinToken withinToken)
        {
            throw new System.NotImplementedException();
        }
    }
}