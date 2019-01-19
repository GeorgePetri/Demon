using System.Collections.Generic;
using System.Text.RegularExpressions;
using Demon.Fody.PointcutExpression.Token;
using Demon.Fody.PointcutExpression.Token.Interface;
using Fody;

namespace Demon.Fody.PointcutExpression
{
    public static class Lexer
    {
        private static readonly Regex Regex = new Regex(
            @"(?<andalso>&&)|(?<orelse>\|\|)|(?<not>!)|(?<execution>Execution\([^()]+\([^()]+\)\s*\))|(?<within>Within\([^()]+\))|(?<pointcut>[a-zA-Z0-9]+\(\))",
            RegexOptions.Compiled);

        public static IEnumerable<IToken> Analyse(string expression)
        {
            var matchedCharactersLength = 0;
            var matches = Regex.Matches(expression);
            foreach (Match match in matches)
            {
                var groups = match.Groups;

                var andAlso = groups["andalso"].Value;
                if (andAlso != "")
                {
                    matchedCharactersLength += andAlso.Length;
                    yield return new AndAlsoToken();
                }

                var orElse = groups["orelse"].Value;
                if (orElse != "")
                {
                    matchedCharactersLength += orElse.Length;
                    yield return new OrElseToken();
                }

                var not = groups["not"].Value;
                if (not != "")
                {
                    matchedCharactersLength += not.Length;
                    yield return new NotToken();
                }

                var execution = groups["execution"].Value;
                if (execution != "")
                {
                    matchedCharactersLength += execution.Length;
                    yield return new ExecutionToken();
                }

                var within = groups["within"].Value;
                if (within != "")
                {
                    matchedCharactersLength += within.Length;
                    yield return new WithinToken();
                }

                var pointcut = groups["pointcut"].Value;
                if (pointcut != "")
                {
                    matchedCharactersLength += pointcut.Length;
                    yield return new PointcutToken();
                }
            }

            var expressionLengthWithoutWhitespace = expression.Replace(" ", "").Length;

            if (matchedCharactersLength < expressionLengthWithoutWhitespace)
                throw new WeavingException($"{expression} is invalid.");
        }
    }
}