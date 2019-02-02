using System;

namespace DemonWeaver
{
    public class WeavingException : Exception
    {
        public WeavingException(string message) : base(message)
        {
        }
    }
}