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

        readonly List<ISym> _syms = new List<ISym>();

        public Parser(List<IToken> tokens) => _tokens = tokens;

        public static List<ISym> Parse(List<IToken> tokens) => new Parser(tokens).Parse();

        public List<ISym> Parse()
        {
            var first = PopToken();

            if (first is LeftParenToken)
                LeftParen();
            else
                throw new WeavingException("Expression must start with \"(\".");

            _syms.Reverse();

            return _syms;
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

        //todo nicer message
        void AndAlso() =>
            HandleAndOr(new AndAlsoSym(), "(and) error", "(and x) error", "(and x y error");

        void Args()
        {
            var stringArgs = new List<StringSym>();

            while (true)
            {
                if (PeekToken() is RightParenToken)
                    break;

                if (PopToken() is StringToken token)
                    stringArgs.Add(new StringSym(token.Value));
                else
                    throw new WeavingException("Args must have only string parameters");
            }

            PushSym(new ArgsSym(stringArgs.Count));
            _syms.AddRange(stringArgs);
        }

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

            PushSym(new NotSym());
        }

        //todo nicer message
        void OrElse() =>
            HandleAndOr(new OrElseSym(), "(or) error", "(or x) error", "(or x y error");

        void Symbol(string value) => PushSym(new SymbolSym(value));

        //todo generalize to function call by arity
        void Within()
        {
            var popped = PopToken();

            if (popped is StringToken token)
            {
                PushSym(new WithinSym());
                PushSym(new StringSym(token.Value));
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

        void PushSym(ISym sym) => _syms.Add(sym);

        //todo vararity
        void HandleAndOr(ISym andOrSym, string errorArity0, string errorArity1, string errorArityMoreThan2)
        {
            if (PeekToken() is RightParenToken)
                throw new WeavingException(errorArity0);

            PushSym(andOrSym);

            var beforeLength = _syms.Count;

            Parse(PopToken());

            if (PeekToken() is RightParenToken)
                throw new WeavingException(errorArity1);

            var added = _syms.GetRange(beforeLength, _syms.Count - beforeLength);
            _syms.RemoveRange(beforeLength, _syms.Count - beforeLength);

            Parse(PopToken());

            _syms.AddRange(added);

            if (!(PeekToken() is RightParenToken))
                throw new WeavingException(errorArityMoreThan2);
        }
    }
}