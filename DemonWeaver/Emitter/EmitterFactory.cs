using System;
using Mono.Cecil.Cil;

namespace DemonWeaver.Emitter
{
    public static class EmitterFactory
    {
        public static Emitter Get(ILProcessor il, Action<Instruction> func) =>
            new Emitter(il, func);

        public static Emitter<TReturn> Get<TReturn>(ILProcessor il, Func<Instruction, TReturn> func) =>
            new Emitter<TReturn>(il, func);

        public static Emitter GetAppend(ILProcessor il) =>
            new Emitter(il, il.Append);
    }
}