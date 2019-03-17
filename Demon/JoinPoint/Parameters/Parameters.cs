using Demon.JoinPoint.Parameters.Interface;

namespace Demon.JoinPoint.Parameters
{
    public class Parameters<T> : IJoinPointParameters
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