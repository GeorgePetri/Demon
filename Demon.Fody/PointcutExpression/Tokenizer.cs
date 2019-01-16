using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Demon.Fody.PointcutExpression
{
    //todo this does very little, might want to move
    //todo define regexs taking into consideration c# namespace rules
    public class Tokenizer
    {
        private readonly string _expression;

        private static readonly Regex Regex = new Regex(@"(?>&&|\|\||!|Execution\([^()]+\([^()]+\)\s*\)|Within\([^()]+\)|[a-zA-Z0-9]+\(\))", RegexOptions.Compiled);

        public Tokenizer(string expression) => _expression = expression;

        //todo error on invalid expression, by comparing the lenght of all matches to the initial expression without whitespace
        public IEnumerable<string> GetTokens()
        {
            var matches = Regex.Matches(_expression);
            foreach (Match match in matches)
                yield return match.Value;
        }

        //todo move this
        //todo don't use nulls for error handling
        public static Regex ProcessWithin(string token)
        {
            //todo make static
            var extractInner = new Regex(@"Within\(\s*(?<inner>[a-zA-Z1-9.*]+)\s*\)", RegexOptions.Compiled);

            var match = extractInner.Match(token);

            //todo test this
            if (!match.Success)
                return null;

            var inner = match.Groups["inner"];
            
            //todo test this
            if(!inner.Success)
                return null;

            var escapeDot = inner.Value.Replace(".", @"\.");

            var replacedDoubleStar = escapeDot.Replace("**", @"[a-zA-Z1-9.]+");
            
            var replacedSingleStar = replacedDoubleStar.Replace("*", @"[a-zA-Z1-9]+");

            return new Regex(replacedSingleStar, RegexOptions.Compiled);
        }
    }
}