using System;
using Mono.Cecil;

namespace DemonWeaver.Data
{
    public abstract class AdviceModel
    {
        protected AdviceModel(MethodDefinition method, Func<MethodDefinition, bool> filterToApply) =>
            (Method, FilterToApply) = (method, filterToApply);

        public MethodDefinition Method { get; }
        public Func<MethodDefinition, bool> FilterToApply { get; }
    }
}