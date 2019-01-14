using System;
using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Static
{
    [Aspect]
    public class ThrowExceptionAspect
    {
        [Before("execution(** AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget.Target(**))")]
        public static void Advice() =>
            throw new ApplicationException("Exception from an aspect");
    }
}