using System.Linq;
using System.Threading.Tasks;
using JsonSSM;
using JsonSSM.Models;
using JsonSSM.Parameters.Delete;
using JsonSSM.Parameters.Put;
using JsonSSM.Results;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonSSMTests
{
    public class DeleteParameterTests
    {
        private FileLoader FileLoader;
        private JsonFlattener JsonFlattener;

        [SetUp]
        public void Setup()
        {
            FileLoader = new FileLoader();
            JsonFlattener = new JsonFlattener();
        }

        public async Task CreateTestData(string path)
        {
            UploadJsonService ujs = new UploadJsonService();

            var result = await ujs.Upload(path);

            Assert.AreEqual(ResultType.Success, result.Type);
        }

        [TestCase("delete_test_input/parameter_to_delete.json")]
        [TestCase("delete_test_input/parameters_to_delete.json")]
        [TestCase("delete_test_input/parameters_to_delete_2.json")]
        public async Task DeleteParametersFromFileTestAsync(string path)
        {
            await CreateTestData(path);

            JToken json = FileLoader.Get(path);
            DataList datalist = JsonFlattener.Flatten(json);

            DeleteCommand deleteClient = new DeleteCommand(datalist);

            Assert.IsNull(deleteClient.GetResults());

            await deleteClient.SendRequest();
            var result = deleteClient.GetResults();

            Assert.AreEqual(ResultType.Success, result.Type);
            Assert.IsTrue(result.Results.All(result => result.Response.InvalidParameters.Count == 0));
        }
    }
}
