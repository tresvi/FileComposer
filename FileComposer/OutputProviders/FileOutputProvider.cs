namespace FileComposer.OutputProviders
{
    public class FileOutputProvider : IOutputProvider, IDisposable
    {
        private readonly StreamWriter _streamWriter;
        private readonly FileStream _fileStream;

        public FileOutputProvider(string path)
        {
            try
            {
                //_fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                _streamWriter = new StreamWriter(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when opening the file {path} for writing. Details: {ex.Message}", ex);
            }
        }

        public void WriteLine(string value)
        {
            _streamWriter.WriteLine(value);
        }

        public void Dispose()
        {
            try
            {
                if (_streamWriter != null)
                {
                    _streamWriter.Close();
                    _streamWriter.Dispose();
                }
            }
            catch { }
        }

    }
}
