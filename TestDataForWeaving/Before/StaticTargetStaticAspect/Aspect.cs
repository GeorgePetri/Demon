using System;
using Demon.Aspect;

namespace TestDataForWeaving.Before.StaticTargetStaticAspect
{
    //todo define rules for public/nonpublic advice
    [Aspect]
    public static class Aspect
    {
        [Before("Within(TestDataForWeaving.Before.StaticTargetStaticAspect.Target.OneInt)")]
        public static void Before()
        {
            throw new Exception("Weaved");
        }
    }
}