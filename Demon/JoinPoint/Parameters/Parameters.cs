namespace Demon.JoinPoint.Parameters
{
    public class Parameters<T>
    {
        public Parameters(T value) => Value = value;

        public T Value { get; set; }
    }
}