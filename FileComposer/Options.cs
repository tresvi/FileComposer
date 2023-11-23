using CommandLine;

namespace FileComposer
{
    internal class Options
    {

        [Option('p', "path", Required = true, HelpText = "Path of the file to compose")]
        public string Path { get; set; }

        [Option('s', "skipInvalidLines", Required = false, HelpText = "Ignore invalid lines")]
        public bool SkipInvalidLines { get; set; }

        [Option('f', "filterPreffix", Required = false, HelpText = "Only lines starting with the specified preffix will be considered")]
        public string FilterPreffix { get; set; }

        [Option('F', "filterSuffix", Required = false, HelpText = "Only lines ending with the specified suffix will be considered")]
        public string FilterSuffix { get; set; }

        [Option('e', "failIf0Replace", Required = false, HelpText = "Fail if no label is found to replace")]
        public bool FailIf0Replace { get; set; }

        [Option('t', "silent", Required = false, HelpText = "If it's set, the result of the replace will not be returned by standar output")]
        public bool Silent { get; set; }

        [Option('i', "ignoreCase", Required = false, HelpText = "Ignore case when find the Keys in the file to compose")]
        public bool IgnoreCase { get; set; }

        [Option('w', "writeFile", Required = false, Default = false ,HelpText = "Overwrite the file especified in the path option")]
        public bool WriteFile { get; set; }

        [Option('j', "jsonCompatible", Required = false, HelpText = "Especify the preffix at the start of a label in de file to compose")]
        public bool JSonCompatible { get; set; }

        [Option('u', "failIfUnreplaced", Required = false, HelpText = "Fail if detects any unreplaced label. Useful to detect configuration values ​​that were forgotten to set in the environment")]
        public bool failIfUnreplaced { get; set; }
        /*
        private const string DEFAULT_SEPARATOR = ":";

        [Option('S', "separator", Required = false, Default = DEFAULT_SEPARATOR, HelpText = "Separator used between the Key and Value in the Key/Value pair")]
        public string Separator { get; set; }

        [Option('l', "labelPreffix", Required = false, Default = "%", HelpText = "Especify the preffix at the start of a label in de file to compose")]
        public bool LabelPreffix { get; set; }

        /*
        [Option('L', "labelSuffix", Required = false, Default = "%", HelpText = "Especify the suffix at the end of a label in de file to compose")]
        public bool LabelSuffix { get; set; }
        */

    }
}
