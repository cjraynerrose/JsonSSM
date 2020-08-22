using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using JsonSSM.Models;
using JsonSSM.Results;

namespace JsonSSM.Parameters.Delete
{
    public class DeleteClient
    {
        private static readonly int MaxPerRequest = 10;
        private DataList DataList { get; }
        private AmazonSimpleSystemsManagementClient SSMClient { get; set; }
        private ResultContainer Result { get; set; }

        public DeleteClient(DataList datalist)
        {
            DataList = datalist;
            SSMClient = new AmazonSimpleSystemsManagementClient(datalist.Meta.Region);
        }

        public ResultContainer GetResults() => Result ?? null;

        public async Task Delete()
        {
            var requests = DeleteParameterRequests();
            var responses = await SendParameterRequests(requests);
            ResolveResultsFromResponses(responses);
        }

        private DeleteParametersRequest[] DeleteParameterRequests()
        {
            var names = DataList.Data
                .Select(p => p.GetParameterName())
                .ToList();

            var numReqs = (names.Count + MaxPerRequest - 1) / MaxPerRequest;
            var requests = new DeleteParametersRequest[numReqs];

            for (int i = 0; i < numReqs; i++)
            {
                var set = names
                    .Skip(i * MaxPerRequest)
                    .Take(MaxPerRequest)
                    .ToList();

                requests[i] = new DeleteParametersRequest
                {
                    Names = set
                };
            }

            return requests;
        }

        private async Task<DeleteParametersResponse[]> SendParameterRequests(DeleteParametersRequest[] requests)
        {
            var responses = new DeleteParametersResponse[requests.Length];

            for (int i = 0; i < requests.Length; i++)
            {
                responses[i] = await SSMClient.DeleteParametersAsync(requests[i]);
            }

            return responses;
        }

        private void ResolveResultsFromResponses(DeleteParametersResponse[] responses)
        {
            var result = responses
                .Select(r => new DeleteResult(r))
                .ToArray();

            Result = new ResultContainer(result);
        }
    }
}