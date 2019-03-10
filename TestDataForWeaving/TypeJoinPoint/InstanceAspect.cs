using Demon.Aspect;

namespace TestDataForWeaving.TypeJoinPoint
{
    [Aspect]
    public class InstanceAspect
    {
        public Demon.JoinPoint.TypeJoinPoint BoundTypeJoinPoint { get; private set; }

        [Pointcut("(within @TestDataForWeaving.TypeJoinPoint.Target.*)")]
        void WithinTypeJoinPoint()
        {
        }

        [Before("(and (within-type-join-point) (args))")]
        public void SingleInt(Demon.JoinPoint.TypeJoinPoint point)
        {
            BoundTypeJoinPoint = point;
        }
    }
}