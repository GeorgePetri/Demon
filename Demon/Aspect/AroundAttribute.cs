using System;

namespace Demon.Aspect
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AroundAttribute : Attribute
    {
        public AroundAttribute(string pointCutExpression)
        {
        }
    }
}