using FileComposer;
using System.Text;

namespace Test_FileComposer.UnitTests
{
    [TestFixture]
    internal class Utils_Test
    {
        private const string TEXTO_A_REEMPLAZAR1 = "Hola %1% tal %2% estas?";
        private const string TEXTO_REEMPLAZADO1 = "Hola que tal como estas?";

        private const string TEXTO_A_REEMPLAZAR2 = "Todo %estado%, aca %accion% un rato";
        private const string TEXTO_REEMPLAZADO2 = "Todo bien, aca caminando un rato";


        [Test]
        [TestCase(TEXTO_A_REEMPLAZAR1, TEXTO_REEMPLAZADO1, "1", "que", "2", "como", false)]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "estado", "bien", "accion", "caminando", false)]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "ESTADO", "bien", "ACCion", "caminando", true)]
        public void ReplaceVariables_ReturnsOK(string textToReplace, string textReplacedOK, string key1, string value1, string key2, string value2, bool ignoreCase)
        {
            Utils utils = new Utils();
            bool atLeastOneReplcement1 = utils.ReplaceVariable(ref textToReplace, key1, value1, ignoreCase);
            bool atLeastOneReplcement2 = utils.ReplaceVariable(ref textToReplace, key2, value2, ignoreCase);
            bool comparison = string.Equals(textToReplace, textReplacedOK, StringComparison.OrdinalIgnoreCase);

            Assert.That(atLeastOneReplcement1 || atLeastOneReplcement2 || comparison,  Is.EqualTo(true));
        }


        [Test]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "estadoXX", "bien", "accionYY", "caminando", false)]
        [TestCase(TEXTO_A_REEMPLAZAR2, TEXTO_REEMPLAZADO2, "ESTADOxx", "bien", "ACCionyy", "caminando", true)]
        public void ReplaceVariables_NoReplacement(string textToReplace, string textReplacedOK, string key1, string value1, string key2, string value2, bool ignoreCase)
        {
            Utils utils = new Utils();
            bool atLeastOneReplcement1 = utils.ReplaceVariable(ref textToReplace, key1, value1, ignoreCase);
            bool atLeastOneReplcement2 = utils.ReplaceVariable(ref textToReplace, key2, value2, ignoreCase);

            Assert.That(atLeastOneReplcement1 || atLeastOneReplcement2, Is.EqualTo(false));
        }


        [Test]
        [TestCase("CONFIG_LOG_ENABLE", "CONFIG_", false, true)]
        [TestCase("CONFIG_LOG_ENABLE", "config_", true, true)]
        [TestCase("CONFIG_LOG_ENABLE", "config_", false, false)]
        [TestCase(" CONFIG_LOG_ENABLE", "CONFIG_", true, false)]
        [TestCase("CONFIG_LOG_ENABLE", " CONFIG_", true, false)]
        public void HasThePreffix(string word, string suffix, bool ignoreCase, bool resultExpected)
        {
            Utils utils = new Utils();
            bool hasTheSuffix = utils.HasThePreffix(word, suffix, ignoreCase);
            Assert.That(hasTheSuffix, Is.EqualTo(resultExpected));
        }


        [Test]
        [TestCase("LOG_ENABLE_CONFIG", "_CONFIG", false, true)]
        [TestCase("LOG_ENABLE_CONFIG", "_config", true, true)]
        [TestCase("LOG_ENABLE_CONFIG", "_config_", false, false)]
        [TestCase("LOG_ENABLE_CONFIG ", "_CONFIG", true, false)]
        [TestCase("LOG_ENABLE_CONFIG", " _CONFIG ", true, false)]
        public void HasTheSuffix(string word, string suffix, bool ignoreCase, bool resultExpected)
        {
            Utils utils = new Utils();
            bool hasTheSuffix = utils.HasTheSuffix(word, suffix, ignoreCase);
            Assert.That(hasTheSuffix, Is.EqualTo(resultExpected));
        }


        [Test]
        [TestCase(@".\..\..\..\TestFiles\TemplateFiles\json1.json")]
        public void GetFileEncoding_OK(string testFilePath)
        {
            Encoding expectedEncoding = Encoding.UTF8;

            Utils utils = new Utils();
            Encoding result = utils.GetFileEncoding(testFilePath);

            Assert.That(result, Is.EqualTo(expectedEncoding));
        }


        [Test]
        public void SaveFile_OK()
        {
            string tempFilePath = $@"{Guid.NewGuid()}.tmp";
            string fileContent = "XXXXXXXXXXXXXXXX";
            Encoding encoding = Encoding.UTF8;

            Utils utils = new Utils();
            utils.SaveFile(tempFilePath, fileContent, encoding);

            bool fileExists = File.Exists(tempFilePath);
            if (fileExists) File.Delete(tempFilePath);

            Assert.IsTrue(fileExists);
        }


        [Test]
        public void SaveFile_ThrowsExceptionProtectedFile()
        {
            string tempFilePath = $@"{Guid.NewGuid()}.tmp";
            string fileContent = "XXXXXXXXXXXXXXXX";
            Encoding encoding = Encoding.UTF8;

            //Creo un archivo y lo bloqueo
            Utils utils = new Utils();
            utils.SaveFile(tempFilePath, fileContent, encoding);
            FileAttributes originalFileAttributes = File.GetAttributes(tempFilePath);
            File.SetAttributes(tempFilePath, originalFileAttributes | FileAttributes.ReadOnly);

            //Assert
            Exception exceptionDetalle = Assert.Throws<Exception>(() => utils.SaveFile(tempFilePath, fileContent, encoding));

            //Lo desbloqueo y lo elimino
            File.SetAttributes(tempFilePath, originalFileAttributes);
            File.Delete(tempFilePath);

            Assert.That(exceptionDetalle.Message, Does.Contain("Error saving the modified file").IgnoreCase);
        }
    }
}
