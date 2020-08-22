using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement.Model;
using JsonSSM.Parameters;
using JsonSSM.Results;

namespace JsonSSM
{
    public class GetQuery : ParameterQuery<GetResult>
    {
        public string Prefix { get; }

        public GetQuery(string prefix, string regionName)
            : base(regionName)
        {
            Prefix = prefix;
        }

        public override async Task SendRequest()
        {
            var request = BuildRequests();
            var response = await SendRequests(request);
            ResolveResults(response);
        }

        private GetParametersByPathRequest BuildRequests()
        {
            var request = new GetParametersByPathRequest
            {
                Path = Prefix,
                WithDecryption = true,
                MaxResults = 10,
                Recursive = true
            };

            return request;
        }

        private async Task<GetParametersByPathResponse[]> SendRequests(GetParametersByPathRequest requests)
        {
            List<GetParametersByPathResponse> responses = new List<GetParametersByPathResponse>();

            var response = await SSMClient.GetParametersByPathAsync(requests);
            responses.Add(response);

            while (response.NextToken != null)
            {
                requests.NextToken = response.NextToken;
                response = await SSMClient.GetParametersByPathAsync(requests);
                responses.Add(response);
            }
            

            return responses.ToArray();
        }

        private void ResolveResults(GetParametersByPathResponse[] responses)
        {
            var results = responses.Select(r => new GetResult(r)).ToArray();
            Results = new ResultContainer<GetResult>(results);
        }
    }
}