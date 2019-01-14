using System;

namespace Demon.Aspect
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PointcutAttribute : Attribute
    {
        public PointcutAttribute(string pointcutExpression)
        {
        }
    }
}