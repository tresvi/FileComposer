using CommandLine;

namespace FileComposer
{
    internal class Options
    {
        private const string DEFAULT_SEPARATOR = ":";

        [Option('p', "path", Required = true, HelpText = "Path of the file to compose")]
        public string Path { get; set; }

        [Option('s', "skipInvalidLines", Required = true, HelpText = "Ignore invalid lines")]
        public bool SkipInvalidLines { get; set; }

        [Option('f', "filterPreffix", Required = true, HelpText = "Only lines starting with the specified preffix will be considered")]
        public string FilterPreffix { get; set; }

        [Option('F', "filterSuffix", Required = true, HelpText = "Only lines ending with the specified suffix will be considered")]
        public string FilterSuffix { get; set; }

        [Option('e', "failIf0Replace", Required = false, HelpText = "Falla si no se encontro ninguna lcave para reemplazar")]
        public bool FailIf0Replace { get; set; }

        [Option('S', "separator", Required = true, Default = DEFAULT_SEPARATOR, HelpText = "Separator used between the Key and Value in the Key/Value pair")]
        public string Separator { get; set; }

        [Option('t', "silent", Required = true, HelpText = "If it's set, the result of the replace will not be returned by standar output")]
        public bool Silent { get; set; }

        [Option('i', "ignoreCase", Required = true, HelpText = "Ignore case when find the Keys in the file to compose")]
        public bool IgnoreCase { get; set; }

        [Option('w', "writeFile", Required = true, Default = false ,HelpText = "Overwrite the file especified in the path option")]
        public bool WriteFile { get; set; }

    }
}
