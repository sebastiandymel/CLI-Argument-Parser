using System;

namespace CommandLineSyntax
{
    public class POSIX_Alias : OptionAliasAttribute
    {
        public POSIX_Alias(char alias) : base(new string(new[] { alias }), String.Empty)
        {           
        }        
    }
}