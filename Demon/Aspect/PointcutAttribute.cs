using System;

namespace Demon.Aspect
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PointcutAttribute
    {
        public PointcutAttribute(string pointCutExpression)
        {
        }
    }
}