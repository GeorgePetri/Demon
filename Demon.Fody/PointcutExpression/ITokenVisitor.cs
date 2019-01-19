using Demon.Fody.PointcutExpression.Token;

namespace Demon.Fody.PointcutExpression
{
    public interface ITokenVisitor<out T>
    {
        T Visit(AndAlsoToken andAlso);
        
        T Visit(ExecutionToken execution);
        
        T Visit(NotToken not);
        
        T Visit(OrElseToken orElse);
        
        T Visit(PointcutToken pointcut);
        
        T Visit(WithinToken withinToken);
    }
}