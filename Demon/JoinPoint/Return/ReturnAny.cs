using Demon.JoinPoint.Return.Interface;

namespace Demon.JoinPoint.Return
{
    public class ReturnAny : IJoinPointReturn
    {
        public object Value { get; set; }
    }
}