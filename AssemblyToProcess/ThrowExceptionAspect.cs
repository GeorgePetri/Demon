using System;
using Demon.Aspect;

namespace AssemblyToProcess
{
    [Aspect]
    public class ThrowExceptionAspect
    {
        [Before("Todo")]
        public static void Advice() =>
            throw new ApplicationException("Exception from an aspect");
    }
}