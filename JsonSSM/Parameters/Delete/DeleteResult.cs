using Amazon.SimpleSystemsManagement.Model;
using JsonSSM.Results;

namespace JsonSSM.Parameters.Delete
{
    public class DeleteResult : AwsResult<DeleteParametersResponse>
    {
        public DeleteResult(DeleteParametersResponse response)
            : base(response)
        {

        }
    }
}