namespace Demon.JoinPoint.Return
{
    public class Return<T>
    {
        public Return(T value) => Value = value;

        public T Value { get; set; }
    }
}