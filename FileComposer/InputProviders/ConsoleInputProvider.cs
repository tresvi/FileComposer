
namespace FileComposer.InputProviders
{
    internal class ConsoleInputProvider : IInputProvider
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
