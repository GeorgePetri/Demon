using System;

namespace Demon.Aspect
{
    //todo, define form for:
    //todo args with generics
    //todo return type
    //todo sync/async/all public/nonpublic
    //todo attributes, for of args and method
    //todo add functionality to test a weather a waving target has a namespace different than current location,it might be unintended
    //todo pbd's
    //todo add debug info 
    //todo fix breakpoints
    [AttributeUsage(AttributeTargets.Class)]
    public class AspectAttribute : Attribute
    {
    }
}