using System;
using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Static
{
    [Aspect]
    public class ThrowExceptionAspect
    {
        [Before("StaticBefore()")]
        public static void Advice() =>
            throw new ApplicationException("Exception from an aspect");
    }
}