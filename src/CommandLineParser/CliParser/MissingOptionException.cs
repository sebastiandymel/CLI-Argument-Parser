using System;

namespace CommandLineSyntax
{
    public class MissingOptionException : ArgumentException
    {
        public MissingOptionException(string msg, string alias) : base(msg, alias)
        {

        }
    }
}