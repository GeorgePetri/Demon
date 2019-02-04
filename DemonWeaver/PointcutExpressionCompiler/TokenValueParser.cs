using DemonWeaver.PointcutExpressionCompiler.Token;
using Mono.Cecil;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public static class TokenValueParser
    {
        //todo impl
        public static string Process(ArgsToken token, MethodDefinition method)
        {
            return null;
        }

        public static string Process(PointcutToken token)
        {
            var value = token.String;

            var withoutParentheses = value.Substring(0, value.Length - 2);

            return withoutParentheses;
        }
    }
}