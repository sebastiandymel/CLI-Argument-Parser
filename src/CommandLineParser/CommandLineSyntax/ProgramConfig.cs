using System;

namespace CommandLineSyntax
{
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
}