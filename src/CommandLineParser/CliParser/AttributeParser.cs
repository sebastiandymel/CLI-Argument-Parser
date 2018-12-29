using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CommandLineSyntax
{
    /// <summary>
    /// Parses arguments intro strongly typed object.
    /// Each property can be represented as separate option.
    /// 
    /// </summary>
    /// <example>     
    /// public class ProgramConfoguration
    ///    {
    ///    [Option]
    ///    [OptionAlias("--days")]
    ///    [OptionAlias("-d")]
    ///    public int DaysSince { get; set; }
    ///
    ///    [Option]
    ///    [OptionAlias("--help")]
    ///    [OptionAlias("-h")]
    ///    public bool ShowHelp { get; set; }
    ///     }
    /// </example>
    public class AttributeParser : IAttributeParser
    {
        private Dictionary<Type, Func<string, object>> customConverters = new Dictionary<Type, Func<string, object>>();

        public T Parse<T>(params string[] arguments) where T : new()
        {
            var result = new T();
            Parse(result, arguments);
            return result;
        }

        public void Parse(object target, params string[] arguments)
        {
            var argumments = arguments.ToList();
            var allProps = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in allProps)
            {
                var mainArg = property.GetCustomAttribute<MainInputAttributeAttribute>();
                if (mainArg != null)
                {
                    property.SetValue(target, ConvertToPropertyType(arguments[0], property.PropertyType));
                    continue;
                }
                var optionAttribute = property.GetCustomAttribute<OptionAttribute>(true);
                if (optionAttribute != null)
                {
                    var alliases = ExtractAlliases(property);
                    if (TryMatch(arguments, alliases, out var alias, out var argument))
                    {
                        if (ShouldTakeNextArg(alias, argument, property, arguments))
                        {
                            var i = Array.IndexOf<string>(arguments, argument);
                            property.SetValue(target, ConvertToPropertyType(arguments[i + 1], property.PropertyType));
                        }

                        var split = Split(alias, argument, property.PropertyType);
                        if (split.Length == 2)
                        {
                            property.SetValue(target, ConvertToPropertyType(split[1], property.PropertyType));
                        }
                        else if (property.PropertyType.IsAssignableFrom(typeof(bool)))
                        {
                            property.SetValue(target, true);
                        }
                    }
                    else if (optionAttribute.IsRequired)
                    {
                        throw new MissingOptionException("Missing parameter", "!");
                    }
                }

                var outputArg = property.GetCustomAttribute<MainOutputAttributeAttribute>();
                if (outputArg != null)
                {
                    property.SetValue(target, ConvertToPropertyType(arguments.Last(), property.PropertyType));
                    continue;
                }
            }
        }

        protected virtual bool ShouldTakeNextArg(Alias alias, string argument, PropertyInfo property, string[] arguments)
        {
            return alias.Splitter == " ";
        }

        protected virtual string[] Split(Alias alias, string argument, Type propertyType)
        {
            return argument.Replace(alias.Name, string.Empty).Split(alias.Splitter);
        }

        protected virtual bool TryMatch(string[] arguments, Alias[] alliases, out Alias foundAlias, out string foundArgument)
        {
            foundArgument = null;
            foundAlias = default(Alias);

            foreach (var arg in arguments)
            {
                foreach (var item in alliases)
                {
                    if (arg.StartsWith(item.Name))
                    {
                        foundArgument = arg;
                        foundAlias = item;
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual Alias[] ExtractAlliases(MemberInfo property)
        {
            var alliases = new List<Alias>();
            var alliasAttributes = property.GetCustomAttributes<OptionAliasAttribute>();
            if (alliasAttributes != null && alliasAttributes.Any())
            {
                alliases.AddRange(alliasAttributes.Select(a => new Alias { Name = a.Alias, Splitter = a.Splitter }));
            }
            else
            {
                alliases.Add(new Alias { Name = property.Name, Splitter = ":" });
            }
            return alliases.ToArray();
        }

        public void RegisterCustomConverter<T>(Func<string, T> converter) where T:class
        {
            this.customConverters.Add(typeof(T), converter);
        }

        protected object ConvertToPropertyType(string input, Type expectedType)
        {
            try
            {
                if (this.customConverters.ContainsKey(expectedType))
                {
                    var customConverter = customConverters[expectedType];
                    return customConverter(input);
                }

                if (expectedType == typeof(double) &&
                    Double.TryParse(input.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleVal))
                {
                    return doubleVal;
                }
                else
                {
                    var converted = Convert.ChangeType(input, expectedType, CultureInfo.InvariantCulture);
                    return converted;
                }
            }
            catch (Exception ex)
            {
                throw new OptionFormatException("Invalid option format", ex);
            }
        }

        protected struct Alias
        {
            public string Name { get; set; }
            public string Splitter { get; set; }
        }        
    }
}