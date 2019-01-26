using Demon.Fody.PointcutExpressionCompiler.Token;

namespace Demon.Fody.PointcutExpressionCompiler
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