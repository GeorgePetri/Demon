using System;

namespace DemonWeaver.Extensions
{
    public static class StandardExtensions
    {
        public static void Apply<T>(this T self, Action<T> action) =>
            action(self);

        public static TReturn Let<T, TReturn>(this T self, Func<T, TReturn> func) =>
            func(self);
    }
}