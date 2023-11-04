namespace FileComposer.OutputProviders
{
    internal class FileOutputProvider : IOutputProvider, IDisposable
    {
        private readonly StreamWriter _sw;

        public FileOutputProvider(string path)
        {
            try
            {
                _sw = new StreamWriter(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when opening the file {path} for writing. Details: {ex.Message}", ex);
            }
        }

        public void WriteLine(string value)
        {
            _sw.WriteLine(value);
        }

        public void Dispose()
        {
            if (_sw == null) return;

            try
            {
                _sw.Close();
                _sw.Dispose();
            }
            catch { }
        }

    }
}
