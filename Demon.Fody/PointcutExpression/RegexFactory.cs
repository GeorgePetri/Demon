using System.Text.RegularExpressions;

namespace Demon.Fody.PointcutExpression
{
    public static class RegexFactory
    {
        private static readonly Regex ExtractInnerWithin = new Regex(@"Within\(\s*(?<inner>[a-zA-Z1-9.*]+)\s*\)", RegexOptions.Compiled);
        
        //todo rename without try, don't muse regs, use string manipulation, don't validate
        public static Regex TryProcessWithin(string token)
        {
            var match = ExtractInnerWithin.Match(token);

            if (!match.Success)
                return null;

            var inner = match.Groups["inner"];
       
            var escapeDot = inner.Value.Replace(".", @"\.");

            var replacedDoubleStar = escapeDot.Replace("**", @"[a-zA-Z1-9.]+");

            var replacedSingleStar = replacedDoubleStar.Replace("*", @"[a-zA-Z1-9]+");
            
            var withEndString = replacedSingleStar + @"$";

            return new Regex(withEndString, RegexOptions.Compiled);
        }
    }
}