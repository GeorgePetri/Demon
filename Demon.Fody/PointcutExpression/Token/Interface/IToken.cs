namespace Demon.Fody.PointcutExpression.Token.Interface
{
    public interface IToken
    {
        void Accept(ITokenVisitor visitor);
    }
}