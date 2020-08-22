using System.Linq;

namespace JsonSSM.Results
{
    public class ResultContainer<TResult> where TResult : Result
    {
        public TResult[] Results { get; }
        public ResultType Type { get; }

        public ResultContainer(params TResult[] results)
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
