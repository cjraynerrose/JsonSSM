using Amazon.SimpleSystemsManagement.Model;

namespace JsonSSM.Models.Results
{
    public class UploadResult
    {
        public PutParameterResponse Response { get; }
        public ResultType Type { get; }

        public UploadResult(PutParameterResponse response)
        {
            Response = response;
            Type = ResolveType();
        }

        private ResultType ResolveType()
        {
            if (Response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return ResultType.Success;
            }

            return ResultType.Failed;
        }
    }
}