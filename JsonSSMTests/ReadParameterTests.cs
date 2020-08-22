using System.Linq;
using System.Threading.Tasks;
using JsonSSM;
using JsonSSM.Results;
using NUnit.Framework;

namespace JsonSSMTests
{
    public class ReadParameterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("/key/path", "eu-west-2")]
        [TestCase("/key", "eu-west-2")]
        public async Task GetParametersFromStore(string prefix, string region)
        {
            GetQuery query = new GetQuery(prefix, region);
            await query.SendRequest();
            var result = query.GetResults();

            Assert.AreEqual(ResultType.Success, result.Type);
            Assert.IsTrue(result.Results[0].Response.Parameters.Count == 1);
            Assert.IsTrue(result.Results[0].Response.Parameters.Any(p => p.Value == "value"));
        }
    }
}
