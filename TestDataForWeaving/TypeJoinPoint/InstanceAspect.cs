using Demon.Aspect;

namespace TestDataForWeaving.TypeJoinPoint
{
    [Aspect]
    public class InstanceAspect
    {
        public Demon.JoinPoint.TypeJoinPoint BoundTypeJoinPoint { get; private set; } 
            
        [Pointcut("Within(TestDataForWeaving.TypeJoinPoint.Target.*)")]
        void WithinTypeJoinPoint()
        {
        }

        [Before("WithinTypeJoinPoint() Args() &&")]
        public void SingleInt(Demon.JoinPoint.TypeJoinPoint point)
        {
            BoundTypeJoinPoint = point;
        }
    }
}