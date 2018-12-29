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
        
        [Option]
        [OptionAlias("--date")]
        [OptionAlias("-d")]
        public DateTime Date { get; set; }
        
        [Option]
        [OptionAlias("--some-value")]
        [OptionAlias("-val")]
        [OptionAlias("-x")]
        public double Value { get; set; }
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

## How to add new options?
### Standard CLR types
Mark property with `[Option]` attribute.
This will create a default alias with the name of property so it can be used right away by executing
> C:\>ProgramName -PropertyName value

To change the alias, mark property with `[OptionAlias]` attribute.
For example `[OptionAlias("--x")]`, then it can be used by executing 
> C:\>ProgramName --x value

### Boolean flags
Boolean properties does not require passing value (it is optional when executing program)
For example
> C:\>Program -SomeFlag
is equivalen to 
> C:\>Program -SomeFlag=true
