using DemonWeaver.PointcutExpressionCompiler.Token;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public interface ITokenVisitor
    {
        void Visit(AndAlsoToken andAlso);
        
        void Visit(NotToken not);
        
        void Visit(OrElseToken orElse);
        
        void Visit(PointcutToken pointcut);
        
        void Visit(WithinToken withinToken);
    }
}