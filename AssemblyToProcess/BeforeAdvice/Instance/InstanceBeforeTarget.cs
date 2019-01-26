namespace AssemblyToProcess.BeforeAdvice.Instance
{
    public class InstanceBeforeTarget
    {
        readonly string _dependency;
        readonly int _ignored;
        readonly int _3;
        readonly int _4;
        readonly int _5;
        readonly int _6;

        public InstanceBeforeTarget()
        {
            
        }
        
        InstanceBeforeTarget(string dependency)
        {
            _dependency = dependency;
        }  
        
        InstanceBeforeTarget(string dependency, int ignored, int a3, int a4, int a5, int a6)
        {
            _dependency = dependency;
            _ignored = ignored;
            _3 = a3;
            _4 = a4;
            _5 = a5;
            _6 = a6;
        }

        public string Target(int parameter) => parameter + _dependency;
    }
}