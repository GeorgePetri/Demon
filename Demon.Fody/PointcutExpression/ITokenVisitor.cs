using Demon.Fody.PointcutExpression.Token;

namespace Demon.Fody.PointcutExpression
{
    public interface ITokenVisitor
    {
        void Visit(AndAlsoToken andAlso);
        
        void Visit(ExecutionToken execution);
        
        void Visit(NotToken not);
        
        void Visit(OrElseToken orElse);
        
        void Visit(PointcutToken pointcut);
        
        void Visit(WithinToken withinToken);
    }
}