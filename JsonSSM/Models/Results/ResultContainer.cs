using System.Linq;

namespace JsonSSM.Models.Results
{
    public class ResultContainer
    {
        public UploadResult[] Results { get; }
        public ResultType Type { get; }

        public ResultContainer(UploadResult[] results)
        {
            Results = results;
            Type = ResolveType();
        }

        private ResultType ResolveType()
        =>
            Results.All(r => r.Type == ResultType.Success) ?
            ResultType.Success :
            ResultType.Failed;

    }
}
