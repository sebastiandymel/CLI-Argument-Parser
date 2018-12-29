using System;

namespace CommandLineSyntax
{
    public class OptionFormatException : Exception
    {
        public OptionFormatException(string msg, Exception inner) : base(msg, inner)
        {

        }
    }
}