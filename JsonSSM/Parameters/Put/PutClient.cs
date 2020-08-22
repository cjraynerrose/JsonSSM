using System.Linq;
using System.Threading.Tasks;

using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

using JsonSSM.Models;
using JsonSSM.Results;

namespace JsonSSM.Parameters.Put
{
    public class PutClient
    {
        private DataList DataList { get; }
        private ResultContainer UploadResult { get; set; }
        private AmazonSimpleSystemsManagementClient SSMClient { get; set; }

        public PutClient(DataList dataList)
        {
            DataList = dataList;
            SSMClient = new AmazonSimpleSystemsManagementClient(DataList.Meta.Region);
        }

        public async Task Upload()
        {
            var requests = CreateParamerterRequests();
            var responses = await SendParameterRequests(requests);
            ResolveResultsFromResponses(responses);
        }

        public ResultContainer GetResults() => UploadResult ?? null;

        private PutParameterRequest[] CreateParamerterRequests()
        {
            var requests = DataList.Data
                .Select(p => new PutParameterRequest
                {
                    Name = p.GetParameterName(),
                    Value = p.GetParameterValue(),
                    Type = p.GetParameterType(),
                    DataType = p.GetParameterDataType(),
                    Overwrite = true
                })
                .ToArray();

            return requests;
        }

        private async Task<PutParameterResponse[]> SendParameterRequests(PutParameterRequest[] requests)
        {
            var responses = new PutParameterResponse[requests.Length];

            for (int i = 0; i < requests.Length; i++)
            {
                responses[i] = await SSMClient.PutParameterAsync(requests[i]);
            }

            return responses;
        }

        private void ResolveResultsFromResponses(PutParameterResponse[] responses)
        {
            var results = responses
                .Select(r => new PutResult(r))
                .ToArray();

            UploadResult = new ResultContainer(results);
        }
    }
}