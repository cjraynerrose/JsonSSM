using System.Linq;

namespace JsonSSM.Results
{
    public class ResultContainer
    {
        public Result[] Results { get; }
        public ResultType Type { get; }

        public ResultContainer(params Result[] results)
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
