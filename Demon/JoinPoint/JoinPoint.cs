using System;
using Demon.JoinPoint.Parameters.Interface;
using Demon.JoinPoint.Return.Interface;

namespace Demon.JoinPoint
{
    //todo add async variant
    //todo add impls for returns: void, generic, any
    //todo add impls for parameters: none, generic 1 to 8, any, 1 to 8 and any after
    public sealed class JoinPoint<TParameters, TReturn>
        where TParameters : IJoinPointParameters
        where TReturn : IJoinPointReturn
    {
        readonly Func<TParameters, TReturn> _body;

        public JoinPoint(TParameters parameters, Func<TParameters, TReturn> body) =>
            (Parameters, _body) = (parameters, body);

        public TParameters Parameters { get; }
        public TReturn Return { get; private set; }

        public void Proceed() =>
            Return = _body(Parameters);
    }
}