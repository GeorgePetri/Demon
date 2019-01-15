using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Demon.Fody.PointcutExpression
{
    //todo this does very little, might want to move
    public class Tokenizer
    {
        private readonly string _expression;

        private static readonly Regex Regex = new Regex(@"(?>&&|\|\||!|Execution\([^()]+\([^()]+\)\s*\)|Within\([^()]+\)|[a-zA-Z0-9]+\(\))", RegexOptions.Compiled);

        public Tokenizer(string expression) => _expression = expression;

        public IEnumerable<string> GetTokens()
        {
            var matches = Regex.Matches(_expression);
            foreach (Match match in matches)
                yield return match.Value;
        }
    }
}