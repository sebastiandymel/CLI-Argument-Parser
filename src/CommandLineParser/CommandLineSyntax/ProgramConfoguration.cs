namespace CommandLineSyntax
{
    public class ProgramConfoguration
    {
        [Option]
        [OptionAlias("--help")]
        [OptionAlias("-h")]
        public bool ShowHelp { get; set; }

        [Option]
        [OptionAlias("--version")]
        [OptionAlias("-v")]
        public bool ShowVersion { get; set; }

        [MainInputAttribute]
        public string ThisIsMainArgument { get; set; }
    }
}