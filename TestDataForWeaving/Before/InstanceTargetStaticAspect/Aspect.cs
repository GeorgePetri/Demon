using System;
using Demon.Aspect;

namespace TestDataForWeaving.Before.InstanceTargetStaticAspect
{
    [Aspect]
    public class Aspect
    {
        [Before("(within @TestDataForWeaving.Before.InstanceTargetStaticAspect.Target.OneInt)")]
        public static void Before()
        {
            throw new Exception("Weaved");
        }
    }
}