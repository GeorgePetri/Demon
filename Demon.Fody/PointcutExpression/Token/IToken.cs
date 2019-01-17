using System.Linq.Expressions;

namespace Demon.Fody.PointcutExpression.Token
{
    public interface IToken
    {
        Expression MakeExpression();
    }
}