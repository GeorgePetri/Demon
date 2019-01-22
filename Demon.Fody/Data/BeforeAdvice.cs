using System;
using Mono.Cecil;

namespace Demon.Fody.Data
{
    public class BeforeAdvice : AdviceModel
    {
        public BeforeAdvice(MethodDefinition method, Func<MethodDefinition, bool> filterToApply) : base(method, filterToApply)
        {
        }
    }
}