
/*
    --path..\..\..\TestFiles\TemplateFiles\json1.json -s -j -w -t
    
*/
using CommandLine;
using FileComposer.InputProviders;
using FileComposer.OutputProviders;
using System.Text;

namespace FileComposer
{
    internal class FileComposerManager
    {
        private readonly IUtils _utils;
        private readonly IInputProvider _inputProvider;
        private readonly IOutputProvider _outputProvider;

        public FileComposerManager(IUtils utils, IInputProvider inputProvider, IOutputProvider outputprovider)
        {
            _utils = utils;
            _inputProvider = inputProvider;
            _outputProvider = outputprovider;
        }

        public void Execute(string[] args)
        {
            Options options = Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o => { }).Value;

            if (string.IsNullOrWhiteSpace(options.Path))
                throw new Exception("The 'Path' parameter can not be null");

            if (!File.Exists(options.Path))
                throw new Exception($"The file especified in 'Path' parameter can not be found: {options.Path}");

            try
            { 
                string fileContent = File.ReadAllText(options.Path);
                string inputLine;

                while ((inputLine = _inputProvider.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(inputLine)) continue;

                    (string key, string value) = GetKeyValuePairs(inputLine, options.Separator);
                    key = key.Trim();

                    if (options.JSonCompatible)
                    {
                        options.SkipInvalidLines = true;
                        key = key.Trim().Trim('"');
                        value = value.Trim().TrimEnd(',').Trim('"');
                    }
                    
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        if (options.SkipInvalidLines)
                            continue;
                        else
                            throw new Exception($"The line '{inputLine}' does not have the right format. The key is null");
                    }

                    bool preffixPresent = _utils.HasThePreffix(key, options.FilterPreffix);
                    bool suffixPresent = _utils.HasTheSuffix(key, options.FilterSuffix);

                    if (!preffixPresent || !suffixPresent) continue;

                    bool keyNotFound = _utils.ReplaceVariable(ref fileContent, key, value, options.IgnoreCase);

                    if (options.FailIf0Replace && keyNotFound)
                        throw new Exception($"The key '{key}' was not foud in the destiny file");
                }

                if (options.WriteFile)
                { 
                    Encoding encoding = _utils.GetFileEncoding(options.Path);
                    _utils.SaveFile(options.Path, fileContent, encoding);
                }

                if (!options.Silent)
                {
                    _outputProvider.WriteLine(fileContent);
                }
            }
            catch (Exception)
            {
                throw;
            }   
        }


        public (string key, string value) GetKeyValuePairs(string keyValuePair, string separator)
        {
            try
            {
                int separatorPosition = keyValuePair.IndexOf(separator);

                if (separatorPosition == -1) return ("","");

                string key  = keyValuePair.Substring(0, separatorPosition);
                string value = keyValuePair.Substring(separatorPosition + 1);

                return (key, value);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
