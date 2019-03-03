using System;
using System.Collections.Generic;
using DemonWeaver.ExpressionCompiler.Token;
using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler
{
    public static class Lexer
    {
        //work with no space in ( and )
        public static IEnumerable<IToken> AnalyseExpression(string expression)
        {
            var split = expression.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            foreach (var value in split)
                yield return Analyse(value);
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
                case "within":
                    return new WithinToken();
                default:
                    throw new WeavingException("Invalid expression.");
            }
        }
    }
}