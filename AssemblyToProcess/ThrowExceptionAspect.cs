using System;
using Demon.Aspect;

namespace AssemblyToProcess
{
    [Aspect]
    public class ThrowExceptionAspect
    {
        [Before("execution(AssemblyToProcess.StaticBeforeTarget.Target)")]
        public static void Advice() =>
            throw new ApplicationException("Exception from an aspect");
    }
}