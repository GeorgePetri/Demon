using System;
using System.Reflection;
using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Static
{
    [Aspect]
    public class ThrowExceptionAspect
    {
        [Before("Within(AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget.Target)")]
        public static void Advice(TypeInfo t) =>
            throw new ApplicationException("Exception from an aspect");
    }
}