namespace AssemblyToProcess.BeforeAdvice.Instance
{
    public class InstanceBeforeTarget
    {
        readonly string _dependency;
        readonly int _ignored;

        InstanceBeforeTarget(string dependency)
        {
            _dependency = dependency;
        }  
        
        public InstanceBeforeTarget(string dependency, int ignored)
        {
            _dependency = dependency;
            _ignored = ignored;
        }

        public string Target(int parameter) => parameter + _dependency;
    }
}