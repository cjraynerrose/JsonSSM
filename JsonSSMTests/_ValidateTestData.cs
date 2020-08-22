using System.IO;
using JsonSSM.Parameters.Put;
using NUnit.Framework;

namespace JsonSSMTests
{
    public class _ValidateTestData
    {
        private FileLoader FileLoader;

        [SetUp]
        public void SetUp()
        {
            FileLoader = new FileLoader();
        }

        [TestCase("create_test_input")]
        [TestCase("delete_test_input")]

        public void ValidateJsonInAllFiles(string directory)
        {
            var files = Directory.GetFiles(directory);

            foreach (var path in files)
            {
                var json = FileLoader.Get(path);
                Assert.NotNull(json);
            }
        }
    }
}
