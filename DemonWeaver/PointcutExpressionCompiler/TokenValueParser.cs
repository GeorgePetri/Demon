using DemonWeaver.PointcutExpressionCompiler.Token;
using Mono.Cecil;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public static class TokenValueParser
    {
        //todo use MethodDefinition here or in visitor?
        //todo throw if not ,* ** a1
        public static string[] Process(ArgsToken token, MethodDefinition method)
        {
            var value = token.String;

            var onlyInner = value.Substring(5, value.Length - 6);

            var noWhitespace = onlyInner.Replace(" ", "");

            var split = noWhitespace.Split(',');

            return split;
        }

        public static string Process(PointcutToken token)
        {
            var value = token.String;

            var withoutParentheses = value.Substring(0, value.Length - 2);

            return withoutParentheses;
        }
    }
}