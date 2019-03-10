using Demon.Aspect;

namespace TestDataForWeaving.Before.InstanceTargetInstanceAspect
{
    [Aspect]
    public class Aspect
    {
        public bool Called { get; set; }

        [Before("(within @TestDataForWeaving.Before.InstanceTargetInstanceAspect.Target.OneInt)")]
        public void Before()
        {
            Called = true;
        }
    }
}