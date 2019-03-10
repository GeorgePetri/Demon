using DemonWeaver.ExpressionCompiler.Sym.Interface;

namespace DemonWeaver.ExpressionCompiler.Sym
{
    public class ArgsSym : ISym
    {
        public ArgsSym(int arity) => Arity = arity;

        public int Arity { get; }
    }
}