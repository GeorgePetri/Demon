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
        readonly Action<TParameters, TReturn> _body;

        public JoinPoint(TParameters parameters, TReturn @return, Action<TParameters, TReturn> body) =>
            (Parameters, _body, Return) = (parameters, body, @return);

        public TParameters Parameters { get; }
        public TReturn Return { get; }

        public void Proceed() => _body(Parameters, Return);
    }
}