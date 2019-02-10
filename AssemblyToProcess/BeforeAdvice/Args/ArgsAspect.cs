using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Args
{
    [Aspect]
    public class ArgsAspect
    {
        public int LastBoundInt { get; set; }

        [Pointcut("Within(AssemblyToProcess.BeforeAdvice.Args.ArgsTarget.*)")]
        void WithinArgsTarget()
        {
        }

        [Before("WithinArgsTarget() Args(i) &&")]
        public void SingleInt(int i)
        {
            LastBoundInt = i;
        }
    }
}