using DemonWeaver.PointcutExpressionCompiler.Token;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public static class TokenValueParser
    {
        public static string[] Process(ArgsToken token)
        {
            var value = token.String;

            var onlyInner = value.Substring(5, value.Length - 6);

            var noWhitespace = onlyInner.Replace(" ", "");

            var split = noWhitespace.Split(',');

            return split.Length == 1 && split[0] == ""
                ? new string[0]
                : split;
        }

        public static string Process(PointcutToken token)
        {
            var value = token.String;

            var withoutParentheses = value.Substring(0, value.Length - 2);

            return withoutParentheses;
        }
        
        public static string Process(WithinToken token)
        {
            var value = token.String;

            var inner = value.Substring(7, value.Length - 8);
            
            var escapeDot = inner.Replace(".", @"\.");

            var replacedDoubleStar = escapeDot.Replace("**", @"[a-zA-Z1-9.]+");

            var replacedSingleStar = replacedDoubleStar.Replace("*", @"[a-zA-Z1-9]+");
            
            var withEndString = $"^{replacedSingleStar}$";

            return withEndString;
        }
    }
}