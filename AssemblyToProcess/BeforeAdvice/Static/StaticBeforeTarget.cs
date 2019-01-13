namespace AssemblyToProcess.BeforeAdvice.Static
{
    public class StaticBeforeTarget
    {
        public string Target(int parameter) => parameter.ToString();
    }
}