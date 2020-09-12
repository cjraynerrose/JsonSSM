using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using JsonSSM;
using JsonSSM.Models;
using JsonSSM.Parameters.Delete;
using JsonSSM.Parameters.Put;
using JsonSSM.Results;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonSSMTests
{
    public class ReadParameterTests
    {
        private FileLoader FileLoader;
        private JsonFlattener JsonFlattener;

        [OneTimeSetUp]
        public async Task Setup()
        {
            FileLoader = new FileLoader();
            JsonFlattener = new JsonFlattener();
            UploadJsonService service = new UploadJsonService();
            await service.Upload("read_test_input/seed_for_read_tests.json");
        }

        [OneTimeTearDown]
        public async Task Teardown()
        {
            JToken json = FileLoader.Get("read_test_input/seed_for_read_tests.json");
            DataList datalist = JsonFlattener.Flatten(json);

            DeleteCommand deleteClient = new DeleteCommand(datalist);

            Assert.IsNull(deleteClient.GetResults());

            await deleteClient.SendRequest();
        }

        [TestCase("/key", "eu-west-2")]
        public async Task GetParametersFromStore(string prefix, string region)
        {
            GetQuery query = new GetQuery(prefix, region);
            await query.SendRequest();
            ResultContainer<GetResult> result = query.GetResults();

            Assert.AreEqual(ResultType.Success, result.Type);
            Assert.IsTrue(result.Results[0].Response.Parameters.Count == 10);
            Assert.IsTrue(result.Results[1].Response.Parameters.Count == 5);
            Assert.IsTrue(result.Results[0].Response.Parameters.All(p => p.Value.StartsWith("value")));
            Assert.IsTrue(result.Results[1].Response.Parameters.All(p => p.Value.StartsWith("value")));
        }

        [TestCase("/key", "eu-west-2")]
        public async Task GetParametersAsJson(string prefix, string region)
        {

        }

        [TestCase("read_test_output/data.json", "/key", "eu-west-2")]
        public async Task GetParametersIntoDocument(string outputPath, string prefix, string region)
        {
            GetQuery query = new GetQuery(prefix, region);
            await query.SendRequest();
            ResultContainer<GetResult> getResult = query.GetResults();

            var command = new CreateFileCommand(getResult, outputPath);

            ResultContainer<FileCreateResult> fileResult = command.Invoke();

            Assert.IsTrue(fileResult.Type == ResultType.Success);

        }
    }
}
