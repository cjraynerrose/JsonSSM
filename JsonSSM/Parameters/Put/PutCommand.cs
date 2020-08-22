using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement.Model;

using JsonSSM.Models;
using JsonSSM.Results;

namespace JsonSSM.Parameters.Put
{
    public class PutCommand : ParameterCommand<PutResult>
    {
        public PutCommand(DataList dataList)
            : base(dataList)
        {
        }

        public override async Task SendRequest()
        {
            var requests = CreateParamerterRequests();
            var responses = await SendParameterRequests(requests);
            ResolveResultsFromResponses(responses);
        }

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

            Results = new ResultContainer<PutResult>(results);
        }
    }
}