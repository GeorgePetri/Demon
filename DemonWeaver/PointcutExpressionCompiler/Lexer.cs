using System.Collections.Generic;
using System.Text.RegularExpressions;
using DemonWeaver.PointcutExpressionCompiler.Token;
using DemonWeaver.PointcutExpressionCompiler.Token.Interface;

namespace DemonWeaver.PointcutExpressionCompiler
{
    public static class Lexer
    {
        static readonly Regex Regex = new Regex(
            @"(?<andalso>&&)|(?<orelse>\|\|)|(?<not>!)|(?<within>Within\([^()]+\))|(?<args>Args\([^()]+\))|(?<pointcut>[a-zA-Z0-9]+\(\))",
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

                var args = groups["args"].Value;
                if (args != "")
                {
                    matchedCharactersLength += args.Length;
                    yield return new ArgsToken(args);
                }

                var not = groups["not"].Value;
                if (not != "")
                {
                    matchedCharactersLength += not.Length;
                    yield return new NotToken();
                }

                var orElse = groups["orelse"].Value;
                if (orElse != "")
                {
                    matchedCharactersLength += orElse.Length;
                    yield return new OrElseToken();
                }

                var pointcut = groups["pointcut"].Value;
                if (pointcut != "")
                {
                    matchedCharactersLength += pointcut.Length;
                    yield return new PointcutToken(pointcut);
                }

                var within = groups["within"].Value;
                if (within != "")
                {
                    matchedCharactersLength += within.Length;
                    yield return new WithinToken(within);
                }
            }

            var expressionLengthWithoutWhitespace = expression.Replace(" ", "").Length;

            if (matchedCharactersLength < expressionLengthWithoutWhitespace)
                throw new WeavingException("Invalid expression.");
        }
    }
}