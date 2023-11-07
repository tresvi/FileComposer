
namespace FileComposer.OutputProviders
{
    public class ConsoleOutputProvider : IOutputProvider
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
