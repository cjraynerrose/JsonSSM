using Amazon.SimpleSystemsManagement.Model;
using JsonSSM.Results;

namespace JsonSSM
{
    public class GetResult : AwsResult<GetParametersByPathResponse>
    {
        public GetResult(GetParametersByPathResponse response)
            : base(response)
        {

        }
    }
}