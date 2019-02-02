using System;

namespace Demon.Fody
{
    public static class StandardExtensions
    {
        public static TReturn Let<T, TReturn>(this T self, Func<T, TReturn> func) =>
            func(self);
    }
}