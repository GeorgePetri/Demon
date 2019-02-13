using Demon.Aspect;

namespace TestDataForWeaving.Args
{
    [Aspect]
    public class ArgsAspect
    {
        public int LastBoundInt { get; set; }

        public string LastBoundString { get; set; }

        public bool EmptyCalled { get; set; }

        [Pointcut("Within(TestDataForWeaving.Args.ArgsTarget.*)")]
        void WithinArgsTarget()
        {
        }

        [Before("WithinArgsTarget() Args(i) &&")]
        public void SingleInt(int i)
        {
            LastBoundInt = i;
        }

        [Before("WithinArgsTarget() Args() &&")]
        public void Empty()
        {
            EmptyCalled = true;
        }

        [Before("WithinArgsTarget() Args(s,*) Args() || &&")]
        public void OptionalStringBinding(string s)
        {
            LastBoundString = s;
        }
    }
}