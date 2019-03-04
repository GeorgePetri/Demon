using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DemonWeaver.ExpressionCompiler.Token;
using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler
{
    public static class Lexer
    {
        //todo unit test this
        static readonly Regex ValidateSymbol = new Regex(@"^[a-zA-Z*][\w.*]*$");

        public static IEnumerable<IToken> AnalyseExpression(string expression)
        {
            var withSpaceAroundLeftParens = expression.Replace("(", " ( ");
            var withSpaceAroundBothParens = withSpaceAroundLeftParens.Replace(")", " ) ");
            var split = withSpaceAroundBothParens.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            foreach (var value in split)
                yield return Analyse(value);

            yield return new EofToken();
        }

        static IToken Analyse(string value)
        {
            switch (value)
            {
                case "(":
                    return new LeftParenToken();
                case ")":
                    return new RightParenToken();
                case "and":
                    return new AndAlsoToken();
                case "or":
                    return new OrElseToken();
                case "not":
                    return new NotToken();
                case "within":
                    return new WithinToken();
                default:
                    return ValidateSymbol.IsMatch(value)
                        ? new SymbolToken(value)
                        : throw new WeavingException("Invalid expression.");
            }
        }
    }
}