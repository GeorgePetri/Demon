namespace DemonWeaver.PointcutExpressionCompiler.Token.Interface
{
    public interface IToken
    {
        void Accept(ITokenVisitor visitor);
    }
}