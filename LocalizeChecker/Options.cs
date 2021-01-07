using CommandLine;

namespace LocalizeChecker
{
    class Options
    {
        [Option("target", Required = true, HelpText = "Enter solution file path to be stretched.")]
        public string FilePath { get; set; }
    }
}
