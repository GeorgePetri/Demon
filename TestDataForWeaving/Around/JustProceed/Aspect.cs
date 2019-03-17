using Demon.Aspect;
using Demon.JoinPoint;
using Demon.JoinPoint.Parameters;
using Demon.JoinPoint.Return;

namespace TestDataForWeaving.Around.JustProceed
{
//    [Aspect]
    public class Aspect
    {
        public bool Called { get; set; }

        [Around("(within @TestDataForWeaving.Around.JustProceed.Target.OneInt)")]
        public void Before(JoinPoint<Parameters<int>,Return<string>> joinPoint)
        {
            Called = true;
            
            joinPoint.Proceed();
        }
    }
}