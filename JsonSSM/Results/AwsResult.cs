using Amazon.Runtime;

namespace JsonSSM.Results
{
    public class AwsResult<T> : Result where T : AmazonWebServiceResponse
    {
        public T Response { get; }

        public AwsResult(T response)
        {
            Response = response;
            ResolveType();
        }

        protected override ResultType ResolveType()
        {
            if (Response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return ResultType.Success;
            }

            return ResultType.Failed;
        }
    }
}
