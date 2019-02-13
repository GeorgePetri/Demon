namespace TestDataForWeaving.Args
{
    public class ArgsTarget
    {
        public string TargetEmpty() => "empty";

        public string TargetInt(int i) => i.ToString();

        public string TargetIntAndString(int i, string s) => i + s;
    }
}