using Demon.Aspect;
using Demon.JoinPoint;
using Demon.JoinPoint.Parameters;
using Demon.JoinPoint.Return;

namespace TestDataForWeaving.Around.NoProceed
{
    [Aspect]
    public class Aspect
    {
        public bool Called { get; set; }
        public Parameters<int> JoinPointParameters { get; set; }
        public Return<string> JoinPointReturn { get; set; }

        [Around("(within @TestDataForWeaving.Around.NoProceed.Target.OneInt)")]
        public void Before(JoinPoint<Parameters<int>, Return<string>> joinPoint)
        {
            Called = true;
            JoinPointParameters = joinPoint.Parameters;
            JoinPointReturn = joinPoint.Return;
        }
    }
}