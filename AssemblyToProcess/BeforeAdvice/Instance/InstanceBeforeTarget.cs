namespace AssemblyToProcess.BeforeAdvice.Instance
{
    public class InstanceBeforeTarget
    {
        public string Target(int parameter) => parameter.ToString();
    }
}