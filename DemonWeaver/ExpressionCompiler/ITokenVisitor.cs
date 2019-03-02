using DemonWeaver.ExpressionCompiler.Token;

namespace DemonWeaver.ExpressionCompiler
{
    public interface ITokenVisitor
    {
        void Visit(AndAlsoToken withinToken);

        void Visit(LeftParenToken withinToken);

        void Visit(RightParenToken withinToken);

        void Visit(WithinToken withinToken);
    }
}