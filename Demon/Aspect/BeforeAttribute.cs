using System;

namespace Demon.Aspect
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute
    {
        public BeforeAttribute(string pointcutExpression)
        {
        }
    }
}