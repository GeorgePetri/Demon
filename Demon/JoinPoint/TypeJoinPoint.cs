namespace Demon.JoinPoint
{
    public class TypeJoinPoint
    {
        public TypeJoinPoint(string name, string fullName) => (Name, FullName) = (name, fullName);

        public string Name { get; }
        public string FullName { get; }
    }
}