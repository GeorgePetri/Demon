using System;

namespace Demon.Aspect
{
    //todo, define form for:
    //todo args with generics
    //todo return type
    //todo sync/async/all public/nonpublic
    //todo attributes, for of args and method
    [AttributeUsage(AttributeTargets.Class)]
    public class AspectAttribute : Attribute
    {
    }
}