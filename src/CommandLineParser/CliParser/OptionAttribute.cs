using System;

namespace CommandLineSyntax
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute(bool isRequired = false)
        {
            IsRequired = isRequired;
        }

        public bool IsRequired { get; }
    }
}