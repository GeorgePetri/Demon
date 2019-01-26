namespace Demon.Fody.PointcutExpressionCompiler.Token.Interface
{
    public interface IToken
    {
        void Accept(ITokenVisitor visitor);
    }
}