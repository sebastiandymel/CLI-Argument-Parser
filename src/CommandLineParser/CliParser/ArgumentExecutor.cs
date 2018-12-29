using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineSyntax
{
    public class ArgumentExecutor: AttributeParser
    {
        public T Execute<T>(params string[] arguments) where T : new()
        {
            var result = new T();
            Execute(result, arguments);
            return result;
        }

        public void Execute(object target, params string[] arguments)
        {
            var argumments = arguments.ToList();
            var allMethods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var pipeline = new List<Action>();

            foreach (var method in allMethods)
            {
                if (method.GetParameters().Any(p =>
                    p.GetCustomAttribute<MainInputAttributeAttribute>() != null ||
                    p.GetCustomAttribute<MainOutputAttributeAttribute>() != null))
                {
                    var methodParams = method.GetParameters();
                    if (methodParams.Length == 2)
                    {
                        var methodArgs = new object[2];
                        if (methodParams[0].GetCustomAttribute<MainInputAttributeAttribute>() != null)
                        {
                            methodArgs[0] = ConvertToPropertyType(arguments[0], method.GetParameters()[0].ParameterType);
                        }
                        if (methodParams[1].GetCustomAttribute<MainOutputAttributeAttribute>() != null)
                        {
                            methodArgs[1] = ConvertToPropertyType(arguments.Last(), method.GetParameters()[1].ParameterType);
                        }
                        pipeline.Add(() => method.Invoke(target, methodArgs));
                    }
                    if (methodParams.Length == 1)
                    {
                        var methodArgs = new object[1];
                        if (methodParams[0].GetCustomAttribute<MainInputAttributeAttribute>() != null)
                        {
                            methodArgs[0] = ConvertToPropertyType(arguments[0], method.GetParameters()[0].ParameterType);
                        }
                        if (methodParams[0].GetCustomAttribute<MainOutputAttributeAttribute>() != null)
                        {
                            methodArgs[0] = ConvertToPropertyType(arguments.Last(), method.GetParameters()[0].ParameterType);
                        }
                        pipeline.Add(() => method.Invoke(target, methodArgs));
                    }
                    continue;
                }

                var optionAttribute = method.GetCustomAttribute<OptionAttribute>(true);
                if (optionAttribute != null)
                {
                    var alliases = ExtractAlliases(method);
                    if (TryMatch(arguments, alliases, out var alias, out var argument))
                    {
                        if (alias.Splitter == " ")
                        {
                            var i = Array.IndexOf<string>(arguments, argument);
                            var input = ConvertToPropertyType(arguments[i + 1], method.GetParameters()[0].ParameterType);
                            Action toExecute = () => method.Invoke(target, new[] { input });
                            pipeline.Add(toExecute);
                            continue;
                        }

                        var split = argument.Split(alias.Splitter);
                        if (split.Length == 2)
                        {
                            var input = ConvertToPropertyType(split[1], method.GetParameters()[0].ParameterType);
                            Action toExecute = () => method.Invoke(target, new[] { input });
                            pipeline.Add(toExecute);
                        }
                        else if (method.GetParameters().Length == 0)
                        {
                            method.Invoke(target, new object[0]);
                        }
                    }
                    else if (optionAttribute.IsRequired)
                    {
                        throw new MissingOptionException("Missing parameter", "!");
                    }
                }

                foreach (var item in pipeline)
                {
                    item();
                }
            }

        }
    }
}