using Demon.JoinPoint.Return.Interface;

namespace Demon.JoinPoint.Return
{
    public class Return<T> : IJoinPointReturn
    {
        public T Value { get; set; }
    }
}