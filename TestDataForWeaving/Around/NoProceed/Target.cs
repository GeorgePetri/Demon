namespace TestDataForWeaving.Around.NoProceed
{
    public class Target
    {
        readonly int _privateMember = 5;

        public string OneInt(int parameter) => _privateMember + parameter.ToString();
    }
}