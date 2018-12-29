using System;

namespace CommandLineSyntax
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class OptionAliasAttribute : Attribute
    {
        public OptionAliasAttribute(string alias) : this(alias, ":")
        {            
        }

        public OptionAliasAttribute(string alias, string splitter)
        {
            Alias = alias;
            Splitter = splitter;
        }

        public string Alias { get; }
        public string Splitter { get; }
    }
}