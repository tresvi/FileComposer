//Agregar --ignoreWrongLines  --Filter   --w (sobreescribir file, si no, devolver el texto por stdout)
// --separator(por default ':')    --failIf0Replace   --silent (no devuelve nada por consola)
//FileComposer


namespace FileComposer
{
    internal class Program
    {
        static int Main(string[] args)
        {

            try
            {
                IUtils utils = new Utils();
                FileComposerManager fileComposer = new FileComposerManager(utils);
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
