using Demon.Aspect;

namespace AssemblyToProcess
{
    //todo define a nicer syntax than public void methods with empty body
    //todo replace with rpn, add more complex ones
    [Aspect]
    public class PointcutExpressions
    {
        [Pointcut("Within(AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget.Target)")]
        void StaticBefore()
        {
        }
    }
}