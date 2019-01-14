using Demon.Aspect;

namespace AssemblyToProcess.BeforeAdvice.Instance
{
    [Aspect]
    public class StatefulInstanceAspect
    {
        public bool AdviceCalled { get; set; }

        [Before("execution(** AssemblyToProcess.BeforeAdvice.Instance.InstanceBeforeTarget.Target(**))")]
        public void Advice()
        {
            AdviceCalled = true;
        }
    }
}