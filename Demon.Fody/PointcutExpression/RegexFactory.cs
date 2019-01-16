using System.Text.RegularExpressions;

namespace Demon.Fody.PointcutExpression
{
    public static class RegexFactory
    {
        private static readonly Regex ExtractInnerWithin = new Regex(@"Within\(\s*(?<inner>[a-zA-Z1-9.*]+)\s*\)", RegexOptions.Compiled);
        
        public static Regex TryProcessWithin(string token)
        {
            var match = ExtractInnerWithin.Match(token);

            //todo test this
            if (!match.Success)
                return null;

            var inner = match.Groups["inner"];

            //todo test this
            if (!inner.Success)
                return null;

            var escapeDot = inner.Value.Replace(".", @"\.");

            var replacedDoubleStar = escapeDot.Replace("**", @"[a-zA-Z1-9.]+");

            var replacedSingleStar = replacedDoubleStar.Replace("*", @"[a-zA-Z1-9]+");

            return new Regex(replacedSingleStar, RegexOptions.Compiled);
        }
    }
}