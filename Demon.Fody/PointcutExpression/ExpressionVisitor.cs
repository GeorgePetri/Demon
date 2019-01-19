using Demon.Fody.PointcutExpression.Token;

namespace Demon.Fody.PointcutExpression
{
    public class ExpressionVisitor : ITokenVisitor
    {
        public void Visit(AndAlsoToken andAlso)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExecutionToken execution)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NotToken not)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(OrElseToken orElse)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(PointcutToken pointcut)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(WithinToken withinToken)
        {
            throw new System.NotImplementedException();
        }
    }
}