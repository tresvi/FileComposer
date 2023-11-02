﻿//Agregar --ignoreWrongLines  --Filter   --w (sobreescribir file, si no, devolver el texto por stdout)
// --separator(por default ':')    --failIf0Replace   --silent (no devuelve nada por consola)
//FileComposer

using CommandLine;
using System.Text;

namespace FileComposer
{
    internal class FileComposerManager
    {

        private readonly IUtils _utils;

        public FileComposerManager(IUtils utils)
        {
            _utils = utils;
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

                while ((inputLine = Console.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(inputLine)) continue;

                    string key, value;
                    (key, value) = GetKeyValuePairs(inputLine, options.Separator);

                    if (string.IsNullOrWhiteSpace(key))
                    {
                        if (!options.SkipInvalidLines)
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
                    Console.WriteLine(fileContent);
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
                int separatorPosition = separator.IndexOf(separator);

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
