using DemonWeaver.PointcutExpressionCompiler.Token;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public static class TokenValueParser
    {
        public static string Process(PointcutToken token)
        {
            var value = token.String;

            var withoutParentheses = value.Substring(0, value.Length - 2);

            return withoutParentheses;
        }
    }
}