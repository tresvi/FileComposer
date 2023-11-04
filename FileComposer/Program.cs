using FileComposer.InputProviders;
using FileComposer.OutputProviders;

namespace FileComposer
{
    internal class Program
    {
        static int Main(string[] args)
        {

            try
            {
                IUtils utils = new Utils();
                //IInputProvider consoleInputProvider = new ConsoleInputProvider();
                IInputProvider consoleInputProvider = new FileInputProvider(@"..\..\..\TestFiles\ValueFiles\SimpleList2.txt");
                IOutputProvider consoleOutputProvider = new ConsoleOutputProvider();

                FileComposerManager fileComposer = new FileComposerManager(utils, consoleInputProvider, consoleOutputProvider);
                fileComposer.Execute(args);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

        }
    }
}
