namespace AssemblyToProcess.BeforeAdvice.Instance
{
    public class InstanceBeforeTarget
    {
        readonly string _dependency;

        public InstanceBeforeTarget(string dependency)
        {
            _dependency = dependency;
        }  
        
        public InstanceBeforeTarget(string dependency, int ignored)
        {
            _dependency = dependency;
        }
        
        public InstanceBeforeTarget(string dependency, bool ignored) : this(dependency)
        {
        }

        public InstanceBeforeTarget() : this(null)
        {
        }

        public string Target(int parameter) => parameter + _dependency;
    }
}