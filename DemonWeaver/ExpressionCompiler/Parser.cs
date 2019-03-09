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
        readonly List<ISym> _syms = new List<ISym>();

        //todo idea: poincuts are funcs with arity
        public Parser(List<IToken> tokens) => _tokens = tokens;

        public static Stack<ISym> Parse(List<IToken> tokens) => new Parser(tokens).Parse();

        public Stack<ISym> Parse()
        {
            var first = PopToken();

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
                case AndAlsoToken _:
                    AndAlso();
                    break;
                case ArgsToken _:
                    Args();
                    break;
                case EofToken _:
                    Eof();
                    break;
                case LeftParenToken _:
                    LeftParen();
                    break;
                case NotToken _:
                    Not();
                    break;
                case OrElseToken _:
                    OrElse();
                    break;
                case SymbolToken symbol:
                    Symbol(symbol.Value);
                    break;
                case WithinToken _:
                    Within();
                    break;
            }
        }

        //todo copy pasted
        //todo impl varargs
        //todo remove hacky reordering between args 
        void AndAlso()
        {
            if (PeekToken() is RightParenToken)
                throw new WeavingException("(and) error"); //todo nicer message

            _stack.Push(new AndAlsoSym());

            var stackCountBeforeFirst = _stack.Count;

            Parse(PopToken());

            var firstStack = new Stack<ISym>();
            while (_stack.Count > stackCountBeforeFirst)
                firstStack.Push(_stack.Pop());

            if (PeekToken() is RightParenToken)
                throw new WeavingException("(and x) error"); //todo nicer message

            Parse(PopToken());

            while (firstStack.Any())
                _stack.Push(firstStack.Pop());

            if (!(PeekToken() is RightParenToken))
                throw new WeavingException("(and x y kdoawda99 error"); //todo nicer message
        }

        //todo do logic here instead?
        void Args() => _stack.Push(new ArgsSym());

        void Eof()
        {
        }

        void LeftParen()
        {
            if (PeekToken() is RightParenToken)
                throw new WeavingException("() error"); //todo nicer message

            while (true)
            {
                var popped = PopToken();

                if (popped is RightParenToken)
                    break;

                Parse(popped);
            }
        }

        void Not()
        {
            if (PeekToken() is RightParenToken)
                throw new WeavingException("(not) error"); //todo nicer message

            _stack.Push(new NotSym());
        }

        //todo copy pasted
        //todo impl varargs
        //todo remove hacky reordering between args 
        void OrElse()
        {
            if (PeekToken() is RightParenToken)
                throw new WeavingException("(or) error"); //todo nicer message

            _stack.Push(new OrElseSym());

            var stackCountBeforeFirst = _stack.Count;

            Parse(PopToken());

            var firstStack = new Stack<ISym>();
            while (_stack.Count > stackCountBeforeFirst)
                firstStack.Push(_stack.Pop());

            if (PeekToken() is RightParenToken)
                throw new WeavingException("(or x) error"); //todo nicer message

            Parse(PopToken());

            while (firstStack.Any())
                _stack.Push(firstStack.Pop());

            if (!(PeekToken() is RightParenToken))
                throw new WeavingException("(or x y kdoawda99 error"); //todo nicer message
        }

        void Symbol(string value) => _stack.Push(new SymbolSym(value));

        //todo generalize to function call by arity
        void Within()
        {
            var popped = PopToken();

            if (popped is StringToken token)
            {
                _stack.Push(new WithinSym());
                _stack.Push(new StringSym(token.Value));
            }
            else
                throw new WeavingException("\"within\" expects a string");
        }

        IToken PopToken()
        {
            var result = _tokens.First();

            _tokens.RemoveAt(0);

            return result;
        }

        IToken PeekToken() => _tokens.First();
    }
}