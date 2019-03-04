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

        //todo idea: poincuts are funcs with arity
        public Parser(List<IToken> tokens) => _tokens = tokens;

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
                case OrElseToken _:
                    OrElse();
                    break;
                case AndAlsoToken _:
                    AndAlso();
                    break;
                case WithinToken _:
                    Within();
                    break;
                case EofToken _:
                    Eof();
                    break;
            }
        }

        //todo copy pasted
        //todo impl varargs
        //todo remove hacky reordering between args 
        void OrElse()
        {
            if (Peek() is RightParenToken)
                throw new WeavingException("(or) error"); //todo nicer message

            _stack.Push(new OrElseSym());

            var stackCountBeforeFirst = _stack.Count;

            Parse(Pop());

            var firstStack = new Stack<ISym>();
            while (_stack.Count > stackCountBeforeFirst)
                firstStack.Push(_stack.Pop());

            Parse(Pop());

            while (firstStack.Any())
                _stack.Push(firstStack.Pop());

            if (!(Peek() is RightParenToken))
                throw new WeavingException("(or x y kdoawda99 error"); //todo nicer message
        }

        //todo copy pasted
        //todo impl varargs
        //todo remove hacky reordering between args 
        void AndAlso()
        {
            if (Peek() is RightParenToken)
                throw new WeavingException("(and) error"); //todo nicer message

            _stack.Push(new AndAlsoSym());

            var stackCountBeforeFirst = _stack.Count;

            Parse(Pop());

            var firstStack = new Stack<ISym>();
            while (_stack.Count > stackCountBeforeFirst)
                firstStack.Push(_stack.Pop());

            Parse(Pop());

            while (firstStack.Any())
                _stack.Push(firstStack.Pop());

            if (!(Peek() is RightParenToken))
                throw new WeavingException("(and x y kdoawda99 error"); //todo nicer message
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

            if (popped is StringToken token)
            {
                _stack.Push(new WithinSym());
                _stack.Push(new StringSym(token.Value));
            }
            else
                throw new WeavingException("\"within\" expects a string");
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