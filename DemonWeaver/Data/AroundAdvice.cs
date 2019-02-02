using System;
using Mono.Cecil;

namespace DemonWeaver.Data
{
    public class AroundAdvice : AdviceModel
    {
        public AroundAdvice(MethodDefinition method, Func<MethodDefinition, bool> filterToApply) : base(method, filterToApply)
        {
        }
    }
}