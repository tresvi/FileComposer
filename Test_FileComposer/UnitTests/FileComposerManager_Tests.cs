using FileComposer;
using FileComposer.InputProviders;
using FileComposer.OutputProviders;

namespace Test_FileComposer.UnitTests
{
    [TestFixture]
    internal class FIleComposerManager_Tests
    {
        [Test]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--ignoreCase" }
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2.txt"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1.json")]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--ignoreCase" }
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2WithComments.txt"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1.json")]
        public void Fill_ReturnsOK(string[] args, string valuesFilePath, string expectedFilePath)
        {
            for (int i = 0; i < args.Count(); i++)
                args[i] = args[i].Trim();

            string resultFilePath = Guid.NewGuid().ToString() + ".json";
            IUtils utils = new Utils();
            IInputProvider fileInputProvider = new FileInputProvider(valuesFilePath);
            IOutputProvider fileOutputProvider = new FileOutputProvider(resultFilePath);

            FileComposerManager fileComposer = new FileComposerManager(utils, fileInputProvider, fileOutputProvider);
            fileComposer.Execute(args);

            ((FileOutputProvider)fileOutputProvider).Dispose();
            string expectedResult = File.ReadAllText(expectedFilePath).Trim();
            string result = File.ReadAllText(resultFilePath).Trim();
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--failIf0Replace"}
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2.txt"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1.json")]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--failIf0Replace" }
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2WithComments.txt"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1.json")]
        public void Fill_Throws0ReplaceException(string[] args, string valuesFilePath, string expectedFilePath)
        {
            for (int i = 0; i < args.Count(); i++)
                args[i] = args[i].Trim();

            string resultFilePath = Guid.NewGuid().ToString() + ".json";
            IUtils utils = new Utils();
            IInputProvider fileInputProvider = new FileInputProvider(valuesFilePath);
            IOutputProvider fileOutputProvider = new FileOutputProvider(resultFilePath);
            FileComposerManager fileComposer = new FileComposerManager(utils, fileInputProvider, fileOutputProvider);

            Exception exceptionDetalle = Assert.Throws<Exception>(() => fileComposer.Execute(args));
            
            ((FileOutputProvider)fileOutputProvider).Dispose();
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            Assert.That(exceptionDetalle.Message, Does.Contain("was not foud in the destiny file").IgnoreCase);
        }


        [Test]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--ignoreCase", "--jsonCompatible" }
            , @".\..\..\..\TestFiles\ValueFiles\json1.json"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1.json")]
        public void Fill_WithJSon_ReturnsOK(string[] args, string valuesFilePath, string expectedFilePath)
        {
            for (int i = 0; i < args.Count(); i++)
                args[i] = args[i].Trim();

            string resultFilePath = Guid.NewGuid().ToString() + ".json";
            IUtils utils = new Utils();
            IInputProvider fileInputProvider = new FileInputProvider(valuesFilePath);
            IOutputProvider fileOutputProvider = new FileOutputProvider(resultFilePath);

            FileComposerManager fileComposer = new FileComposerManager(utils, fileInputProvider, fileOutputProvider);
            fileComposer.Execute(args);

            ((FileOutputProvider)fileOutputProvider).Dispose();
            string expectedResult = File.ReadAllText(expectedFilePath).Trim();
            string result = File.ReadAllText(resultFilePath).Trim();
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--silent" }
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2.txt")]
        public void Fill_SilentMode_ReturnsOK(string[] args, string valuesFilePath)
        {
            for (int i = 0; i < args.Count(); i++)
                args[i] = args[i].Trim();

            string resultFilePath = Guid.NewGuid().ToString() + ".json";
            IUtils utils = new Utils();
            IInputProvider fileInputProvider = new FileInputProvider(valuesFilePath);
            IOutputProvider fileOutputProvider = new FileOutputProvider(resultFilePath);

            FileComposerManager fileComposer = new FileComposerManager(utils, fileInputProvider, fileOutputProvider);
            fileComposer.Execute(args);

            ((FileOutputProvider)fileOutputProvider).Dispose();
            string result = File.ReadAllText(resultFilePath).Trim();
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            Assert.That(result, Is.EqualTo(""));
        }


        [Test]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--ignoreCase", "--filterPreffix", "CONFIG_" }
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2.txt"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1ToPreffixFilterTest.json")]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1.json", "--ignoreCase", "--filterPreffix", "CONFIG_" }
            , @".\..\..\..\TestFiles\ValueFiles\SimpleList2WithComments.txt"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1ToPreffixFilterTest.json")]
        public void Fill_InputWithPreffix_ReturnsOK(string[] args, string valuesFilePath, string expectedFilePath)
        {
            for (int i = 0; i < args.Count(); i++)
                args[i] = args[i].Trim();

            string resultFilePath = Guid.NewGuid().ToString() + ".json";
            IUtils utils = new Utils();
            IInputProvider fileInputProvider = new FileInputProvider(valuesFilePath);
            IOutputProvider fileOutputProvider = new FileOutputProvider(resultFilePath);

            FileComposerManager fileComposer = new FileComposerManager(utils, fileInputProvider, fileOutputProvider);
            fileComposer.Execute(args);

            ((FileOutputProvider)fileOutputProvider).Dispose();
            string expectedResult = File.ReadAllText(expectedFilePath).Trim();
            string result = File.ReadAllText(resultFilePath).Trim();
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1ToSuffixFilterTest.json", "--ignoreCase", "--filterSuffix", "Var1", "--jsonCompatible" }
            , @".\..\..\..\TestFiles\ValueFiles\json1ToSuffixFilterTest.json"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1ToSuffixFilterTest.json")]
        [TestCase(new string[] { "--path", @".\..\..\..\TestFiles\TemplateFiles\json1ToSuffixFilterTest.json", "--ignoreCase", "--filterSuffix", "Var1", "--jsonCompatible" }
            , @".\..\..\..\TestFiles\ValueFiles\json1ToSuffixFilterTest.json"
            , @".\..\..\..\TestFiles\ExpectedResultsFiles\json1ToSuffixFilterTest.json")]
        public void Fill_InputWithSuffix_ReturnsOK(string[] args, string valuesFilePath, string expectedFilePath)
        {
            for (int i = 0; i < args.Count(); i++)
                args[i] = args[i].Trim();

            string resultFilePath = Guid.NewGuid().ToString() + ".json";
            IUtils utils = new Utils();
            IInputProvider fileInputProvider = new FileInputProvider(valuesFilePath);
            IOutputProvider fileOutputProvider = new FileOutputProvider(resultFilePath);

            FileComposerManager fileComposer = new FileComposerManager(utils, fileInputProvider, fileOutputProvider);
            fileComposer.Execute(args);

            ((FileOutputProvider)fileOutputProvider).Dispose();
            string expectedResult = File.ReadAllText(expectedFilePath).Trim();
            string result = File.ReadAllText(resultFilePath).Trim();
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

    }
}
