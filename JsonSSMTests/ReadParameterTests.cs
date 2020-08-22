using System.Threading.Tasks;
using NUnit.Framework;

namespace JsonSSMTests
{
    public class ReadParameterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public async Task GetParametersFromStore(string prefix)
        {
            var getClient = new GetClient(prefix);

        }
    }
}
