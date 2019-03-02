namespace DemonWeaver.ExpressionCompiler.Token.Interface
{
    public interface IToken
    {
        void Accept(ITokenVisitor visitor);
    }
}