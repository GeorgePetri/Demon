using Demon.Aspect;

namespace TestDataForWeavingDependency
{
    [Aspect]
    public class AspectInDependency
    {
        public int LastBoundInt { get; set; }

        [Before("Within(TestDataForWeaving.DependencyAspectTarget.Target.*) Args(i) &&")]
        public void TargetInt(int i)
        {
            LastBoundInt = i;
        }
    }
}