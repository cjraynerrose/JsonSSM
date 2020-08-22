namespace JsonSSM.Results
{
    public abstract class Result
    {
        public ResultType Type { get; }

        public Result() { }

        protected abstract ResultType ResolveType();
    }
}
