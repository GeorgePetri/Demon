using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Instance
{
    [Aspect]
    public class StatefulInstanceAspect
    {
        public bool AdviceCalled { get; set; }

        [Before("Within(AssemblyToProcess.BeforeAdvice.Instance.InstanceBefore*.Target)")]
        public void Advice()
        {
            AdviceCalled = true;
        }
    }
}