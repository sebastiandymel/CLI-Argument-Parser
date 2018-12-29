using System;
using System.Reflection;

namespace CommandLineSyntax
{
    public class POSIXParser: AttributeParser
    {
        // POSIX (Portable Operating System Interface for UNIX)
        //- Arguments are options if they begin with a hyphen delimiter(‘-’).
        //- Multiple options may follow a hyphen delimiter in a single token if the options do not take arguments.Thus, ‘-abc’ is equivalent to ‘-a -b -c’.
        //- Option names are single alphanumeric characters(as for isalnum; see Classification of Characters).
        //- Certain options require an argument.For example, the ‘-o’ command of the ld command requires an argument—an output file name.
        //- An option and its argument may or may not appear as separate tokens. (In other words, the whitespace separating them is optional.) Thus, ‘-o foo’ and ‘-ofoo’ are equivalent.
        //- Options typically precede other non-option arguments.


        protected override bool TryMatch(string[] arguments, Alias[] alliases, out Alias foundAlias, out string foundArgument)
        {
            foundArgument = null;
            foundAlias = default(Alias);

            foreach (var arg in arguments)
            {
                if (arg.StartsWith("-"))
                {
                    for (int i = 1; i < arg.Length; i++)
                    {
                        foreach (var item in alliases)
                        {
                            if (arg[i] == item.Name[0])
                            {
                                foundArgument = arg;
                                foundAlias = item;
                                return true;
                            }
                        }
                    }
                    
                }
                
            }

            return false;
        }

        protected override string[] Split(Alias alias, string argument, Type expectedType)
        {
            var newargument = argument.Replace("-", String.Empty);
            if (expectedType != typeof(bool) && alias.Splitter == String.Empty && newargument.Length > 1)
            {
                var result = new string[2];
                result[0] = alias.Name;
                result[1] = newargument.Substring(1, newargument.Length - 1);
                return result;
            }
            else
            {
                return base.Split(alias, argument, expectedType);
            }
        }

        protected override bool ShouldTakeNextArg(Alias alias, string argument, PropertyInfo property, string[] arguments)
        {
            var i = Array.IndexOf<string>(arguments, argument);
            if (arguments.Length > (i + 1) && property.PropertyType != typeof(bool) && argument.IndexOf(alias.Name[0]) == (argument.Length -1))
            {
                return true;
            }
            else
            {
                return base.ShouldTakeNextArg(alias, argument, property, arguments);
            }            
        }
    }
}