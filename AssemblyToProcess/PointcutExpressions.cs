using Demon.Aspect;

namespace AssemblyToProcess
{
    //todo, define form for:
    //todo args with generics
    //todo return type
    //todo sync/async/all public/nonpublic
    //todo attributes, for of args and method
    [Aspect]
    public class PointcutExpressions
    {
        [Pointcut("Within(AssemblyToProcess.BeforeAdvice.Static.StaticBeforeTarget.Target)")]
        void StaticBefore()
        {
        }

        [Pointcut("Args()")]
        void ArgsEmpty()
        {
        }

        [Pointcut("Args(i)")]
        void ArgsWithOneBound(int i)
        {
        }

        [Pointcut("Args(i,s)")]
        void ArgsWithTwoBound(int i, string s)
        {
        }

        [Pointcut("Args(i,*)")]
        void ArgsWithOneBoundAndAnotherAny()
        {
        }

        [Pointcut("Args(i,**)")]
        void ArgsWithOneBoundAndAnyOfAnyType()
        {
        }
    }
}