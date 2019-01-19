namespace Demon.Fody.PointcutExpression.Token.Interface
{
    public interface IToken
    {
        T Accept<T>(ITokenVisitor<T> visitor);
    }
}