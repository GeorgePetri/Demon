namespace AssemblyToProcess.BeforeAdvice.Args
{
    public class ArgsTarget
    {
        public string TargetInt(int i) => i.ToString();
        
        public string TargetIntAndString(int i, string s) => i + s;
    }
}