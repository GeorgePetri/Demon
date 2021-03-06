using Demon.Aspect;

namespace TestDataForWeaving.Args
{
    [Aspect]
    public class ArgsAspect
    {
        public int LastBoundInt { get; set; }

        public string LastBoundString { get; set; }

        public ComplexClass LastBoundComplex { get; set; }

        public bool EmptyCalled { get; set; }

        [Pointcut("(within @TestDataForWeaving.Args.ArgsTarget.*)")]
        void WithinArgsTarget()
        {
        }

        [Before("(and (within-args-target) (args @i))")]
        public void SingleInt(int i)
        {
            LastBoundInt = i;
        }

        [Before("(and (within-args-target) (args))")]
        public void Empty()
        {
            EmptyCalled = true;
        }

        [Before("(and (within-args-target) (or (args @s @*) (args)))")]
        public void OptionalStringBinding(string s)
        {
            LastBoundString = s;
        }
        
        [Before("(and (within-args-target) (or (args @c) (args)))")]
        public void OptionalComplex(ComplexClass c)
        {
            LastBoundComplex = c;
        }
    }
}