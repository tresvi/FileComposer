namespace FileComposer.InputProviders
{
    public class FileInputProvider : IInputProvider, IDisposable
    {
        private readonly StreamReader _sr;

        public FileInputProvider(string path)
        {
            try
            {
                _sr = new StreamReader(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when opening the file {path}. Details: {ex.Message} for reading", ex);
            }

        }

        public string ReadLine()
        {
            return _sr.ReadLine();
        }

        public void Dispose()
        {
            if (_sr == null) return;

            try
            {
                _sr.Close();
                _sr.Dispose();
            }
            catch { }
        }

    }
}
