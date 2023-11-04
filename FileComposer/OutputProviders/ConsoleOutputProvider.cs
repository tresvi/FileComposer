
namespace FileComposer.OutputProviders
{
    internal class ConsoleOutputProvider : IOutputProvider
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
