using System;
using System.Reflection;
using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Static
{
    [Aspect]
    public class ThrowExceptionAspect
    {
        [Before("StaticBefore()")]
        public static void Advice(TypeInfo t) =>
            throw new ApplicationException("Exception from an aspect");
    }
}