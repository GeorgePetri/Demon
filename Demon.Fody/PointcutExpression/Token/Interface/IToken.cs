using System.Linq.Expressions;

namespace Demon.Fody.PointcutExpression.Token.Interface
{
    public interface IToken
    {
        Expression MakeExpression();
    }
}