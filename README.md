# CLI-Argument-Parser
Library to parse command line arguments - attributes based approach. C# and .netCore

## How to use it?

Create a class to hold desired configuration settings, mark with attributes:

```csharp
    public class ProgramConfig
    {
        [Option]
        [OptionAlias("--help")]
        [OptionAlias("-h")]
        public bool ShowHelp { get; set; }

        [Option]
        [OptionAlias("--version")]
        [OptionAlias("-v")]
        public bool ShowVersionDetails { get; set; }

        [Option]
        [OptionAlias("--age-of-client")]
        [OptionAlias("-a")]
        public int Age { get; set; }
    }
```


Parse it:
```csharp
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new AttributeParser();
            var configuration = parser.Parse<ProgramConfig>(args);
        }
    }
```

Use it in application
```csharp
if (configuration.ShowHelp)
    {
        Console.WriteLine("HEEELP");
    }
```
