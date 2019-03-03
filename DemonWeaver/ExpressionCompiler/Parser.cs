using System.Collections.Generic;
using System.Linq;
using DemonWeaver.ExpressionCompiler.Sym;
using DemonWeaver.ExpressionCompiler.Sym.Interface;
using DemonWeaver.ExpressionCompiler.Token;
using DemonWeaver.ExpressionCompiler.Token.Interface;

namespace DemonWeaver.ExpressionCompiler
{
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

            if (first is LeftParenToken)
                LeftParen();
            else
                throw new WeavingException("Expression must start with \"(\".");

            return _stack;
        }

        void Parse(IToken token)
        {
            switch (token)
            {
                case LeftParenToken _:
                    LeftParen();
                    break;
                case WithinToken _:
                    Within();
                    break;
                case EofToken _:
                    Eof();
                    break;
            }
        }

        void LeftParen()
        {
            if (Peek() is RightParenToken)
                throw new WeavingException("() error"); //todo nicer message

            while (true)
            {
                var popped = Pop();

                if (popped is RightParenToken)
                    break;

                Parse(popped);
            }
        }

        //todo generalize to function call by arity
        void Within()
        {
            var popped = Pop();

            if (popped is SymbolToken symbol)
                _stack.Push(new WithinSym(symbol.Value));
            else
                throw new WeavingException("\"within\" expects a symbol"); //todo is symbol name good here?
        }

        void Eof()
        {
        }

        IToken Pop()
        {
            var result = _tokens.First();

            _tokens.RemoveAt(0);

            return result;
        }

        IToken Peek() => _tokens.First();
    }
}

//( and ( within **.get* ) endpoints )
//endpoints = ( within x.y.controllers.*.* )