using Amazon.SimpleSystemsManagement.Model;
using JsonSSM.Results;

namespace JsonSSM.Parameters.Put
{
    public class PutResult : AwsResult<PutParameterResponse>
    {
        public PutResult(PutParameterResponse response)
            : base(response)
        {

        }
    }
}