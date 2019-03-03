using System.Collections.Generic;
using System.Linq;
using DemonWeaver.ExpressionCompiler.Sym.Interface;
using DemonWeaver.ExpressionCompiler.Token;
using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler
{
    //handle eof, nulls, etc
    public class Parser
    {
        readonly List<IToken> _tokens;
        readonly Stack<ISym> _stack = new Stack<ISym>();

        public Parser(List<IToken> tokens)
        {
            _tokens = tokens;
        }

        public Stack<ISym> Parse()
        {
            var first = Pop();

            if (first is LeftParenToken leftParenToken)
                LeftParen(leftParenToken);
            else
                throw new WeavingException("Expression must start with \"(\".");

            return _stack;
        }

        void LeftParen(LeftParenToken parenToken)
        {
            var first = Pop();
            
            if(first is RightParenToken)
                throw new WeavingException("() error"); //todo nicer message

            while (true)
            {
                
            }
            
        }

        IToken Pop()
        {
            var result = _tokens.First();
            
            _tokens.RemoveAt(0);

            return result;
        }
    }
}

//( and ( within **.get* ) endpoints )
//endpoints = ( within x.y.controllers.*.* )