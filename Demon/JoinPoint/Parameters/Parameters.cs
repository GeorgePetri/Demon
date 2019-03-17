namespace Demon.JoinPoint.Parameters
{
    public class Parameters<T>
    {
        public Parameters(T value, string name)
        {
            Value = value;
            Name = name;
        }

        public T Value { get; set; }
        
        public string Name { get; }
    }
}