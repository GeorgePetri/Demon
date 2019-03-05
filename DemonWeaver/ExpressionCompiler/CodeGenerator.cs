using System.Collections.Generic;
using System.Linq.Expressions;
using DemonWeaver.ExpressionCompiler.Sym;
using DemonWeaver.ExpressionCompiler.Sym.Interface;

namespace DemonWeaver.ExpressionCompiler
{
    public class CodeGenerator
    {
        readonly Stack<ISym> _syms;
        readonly Stack<Expression> _stack = new Stack<Expression>();

        public CodeGenerator(Stack<ISym> syms) => _syms = syms;

        public void Generate()
        {
            GenerateNext();
        }

        void GenerateNext()
        {
            switch (_syms.Pop())
            {
                case AndAlsoSym _:
                    AndAlso();
                    break;
                case NotSym _:
                    Not();
                    break;
                case OrElseSym _:
                    OrElse();
                    break;
                case StringSym _:
                    String();
                    break;
                case SymbolSym _:
                    Symbol();
                    break;
                case WithinSym _:
                    Within();
                    break;
            }
        }

        void AndAlso()
        {
            throw new System.NotImplementedException();
        }

        void Not()
        {
            throw new System.NotImplementedException();
        }

        void OrElse()
        {
            throw new System.NotImplementedException();
        }

        void String()
        {
            throw new System.NotImplementedException();
        }

        void Symbol()
        {
            throw new System.NotImplementedException();
        }

        void Within()
        {
            throw new System.NotImplementedException();
        }
    }
}