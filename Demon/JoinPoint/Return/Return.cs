using Demon.JoinPoint.Return.Interface;

namespace Demon.JoinPoint.Return
{
    public class Return<T> : IJoinPointReturn
    {
        public Return(T value) => Value = value;

        public T Value { get; set; }
    }
}