using System;

namespace CommandLineSyntax
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MainInputAttributeAttribute: OptionAttribute
    {

    }
}