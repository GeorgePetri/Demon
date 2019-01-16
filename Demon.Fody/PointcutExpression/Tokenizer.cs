using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Demon.Fody.PointcutExpression
{
    //todo this does very little, might want to move
    //todo define regexs taking into consideration c# fullname rules
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

    }
}