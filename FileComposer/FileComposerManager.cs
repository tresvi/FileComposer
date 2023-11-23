/*
    --path..\..\..\TestFiles\TemplateFiles\json1.json -s -j -w -t    
*/
using CommandLine;
using FileComposer.InputProviders;
using FileComposer.OutputProviders;
using System.Text;
using System.Text.RegularExpressions;

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
                
            if (!File.Exists(options.Path))
                throw new Exception($"The file especified in 'Path' parameter can not be found: {options.Path}");

            try
            { 
                string configFileContent = File.ReadAllText(options.Path);
                string inputLine;

                while ((inputLine = _inputProvider.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(inputLine)) continue;
                    if (inputLine.Trim().StartsWith(COMMENT_PREFFIX)) continue;

                    (string key, string value) = GetKeyValuePairs(inputLine, DEFAULT_KEY_VALUE_SEPARATOR);
                    key = key.Trim().Trim('"');

                    if (options.JSonCompatible)
                    {
                        if (value.Trim() == "{") continue;
                        options.SkipInvalidLines = true;
                        value = value.Trim().TrimEnd(',').Trim('"');
                    }
                    else
                    {
                        value = value.Trim();
                        if (value.StartsWith('"') && value.EndsWith('"')) value = value.Trim('"');
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

                    bool keyWasReplaced = _utils.ReplaceVariable(ref configFileContent, key, value, options.IgnoreCase);

                    if (options.FailIf0Replace && !keyWasReplaced)
                        throw new Exception($"The key '{key}' was not foud in the destiny file");
                }

                if (options.failIfUnreplaced)
                    CheckUnreplacedTags(configFileContent);
                    

                if (options.WriteFile)
                { 
                    Encoding encoding = _utils.GetFileEncoding(options.Path);
                    _utils.SaveFile(options.Path, configFileContent, encoding);
                }

                if (!options.Silent)
                {
                    _outputProvider.WriteLine(configFileContent);
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


        public void CheckUnreplacedTags(string fileContent)
        {
            Regex regex = new Regex(@"%([^%]+)%");
            string varsNotReplced = "";

            using (StringReader reader = new StringReader(fileContent))
            {
                string line;                
                int lineNumber = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    MatchCollection matches = regex.Matches(line);

                    foreach (Match match in matches)
                    {
                        varsNotReplced += $", key:{match.Groups[1].Value} (line {lineNumber})";
                    }
                    lineNumber++;
                }
            }

            if (varsNotReplced != "")
                throw new Exception($"Unreplaced variables was found{varsNotReplced}");
        }

    }
}
