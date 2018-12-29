using System;

namespace CommandLineSyntax
{
    // Microsoft CLI syntax:
    // https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-R2-and-2012/cc771080(v=ws.11)

    class Program
    {
        static void Main(string[] args)
        {
            var parser = new AttributeParser();
            var configuration = parser.Parse<ProgramConfig>(args);

            if (configuration.ShowHelp)
            {
                Console.WriteLine("HEEELP");
            }

            Console.ReadLine();
        }
    }
}
