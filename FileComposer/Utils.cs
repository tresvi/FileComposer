using System.Text;

namespace FileComposer
{

    public interface IUtils
    {
        public Encoding GetFileEncoding(string filePath);
        public bool ReplaceVariable(ref string textToReplace, string key, string value, bool ignoreCase);
        public void SaveFile(string filePath, string content, Encoding encoding);
        public bool HasThePreffix(string word, string preffix);
        public bool HasTheSuffix(string word, string suffix);
    }


    public class Utils : IUtils
    {
        public bool ReplaceVariable(ref string textToReplace, string key, string value, bool ignoreCase)
        {
            try
            {
                StringComparison comparisonType = (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

                bool textIsPresent = textToReplace.Contains($"%{key}%", comparisonType);
                if (!textIsPresent) return false;
                
                textToReplace = textToReplace.Replace($"%{key}%", value, comparisonType);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error replacing the key '{key}' with the value '{value}'. Details: {ex.Message}");
            }
        }


        public Encoding GetFileEncoding(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath, detectEncodingFromByteOrderMarks: true))
                {
                    return reader.CurrentEncoding;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error determining the file encoding of '{filePath}'. Details: {ex.Message}");
            }
        }


        public void SaveFile(string filePath, string content, Encoding encoding)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, encoding))
                {
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving the modified file '{filePath}'. Details: {ex.Message}");
            }
        }

        public bool HasThePreffix(string word, string preffix)
        {
            throw new NotImplementedException();
        }

        public bool HasTheSuffix(string word, string suffix)
        {
            throw new NotImplementedException();
        }

    }
}
