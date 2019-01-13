namespace AssemblyToProcess.BeforeAdvice.Instance
{
    public class InstanceBeforeTarget
    {
        private readonly string _dependency;

        public InstanceBeforeTarget(string dependency)
        {
            _dependency = dependency;
        }

        public string Target(int parameter) => parameter + _dependency;
    }
}