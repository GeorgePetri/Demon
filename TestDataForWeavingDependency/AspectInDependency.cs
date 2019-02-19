using Demon.Aspect;

namespace TestDataForWeavingDependency
{
    //todo test args when fixed, unit test
    [Aspect]
    public class AspectInDependency
    {
        public bool AdviceCalled { get; set; }

        [Before("Within(TestDataForWeaving.DependencyAspectTarget.Target.*)")]
        public void TargetInt(int i)
        {
            AdviceCalled = true;
        }
    }
}