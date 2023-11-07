
/*
    --path..\..\..\TestFiles\TemplateFiles\json1.json -s -j -w -t
    
*/
using CommandLine;
using FileComposer.InputProviders;
using FileComposer.OutputProviders;
using System.Diagnostics;
using System.Text;

namespace FileComposer
{
    public class FileComposerManager
    {
        private readonly IUtils _utils;
        private readonly IInputProvider _inputProvider;
        private readonly IOutputProvider _outputProvider;
        private const char COMMENT_PREFFIX = '#';
        private const char DEFAULT_KEY_VALUE_SEPARATOR = ':';

        public FileComposerManager(IUtils utils, IInputProvider inputProvider, IOutputProvider outputprovider)
        {
            _utils = utils;
            _inputProvider = inputProvider;
            _outputProvider = outputprovider;
        }

        public void Execute(string[] args)
        {
            Options options = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => { })
                .WithNotParsed(HandleParseError).Value;
                

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
                    if (inputLine.Trim().StartsWith(COMMENT_PREFFIX)) continue;

                    (string key, string value) = GetKeyValuePairs(inputLine, DEFAULT_KEY_VALUE_SEPARATOR);
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

                    bool preffixPresent = _utils.HasThePreffix(key, options.FilterPreffix, options.IgnoreCase);
                    bool suffixPresent = _utils.HasTheSuffix(key, options.FilterSuffix, options.IgnoreCase);

                    if (!preffixPresent || !suffixPresent) continue;

                    bool keyWasReplaced = _utils.ReplaceVariable(ref fileContent, key, value, options.IgnoreCase);

                    if (options.FailIf0Replace && !keyWasReplaced)
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


        static void HandleParseError(IEnumerable<Error> errs)
        {
            string errDescrption = "";
            foreach (Error error in errs) 
            {
                errDescrption += error.ToString() + "|";
            }

            throw new Exception($"Error when parse arguments. Details: {errDescrption}");
        }

        public (string key, string value) GetKeyValuePairs(string keyValuePair, char separator)
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
