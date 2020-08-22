using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonSSM.Results;

namespace JsonSSM.Parameters
{
    public abstract class Command<TResult> where TResult : Result
    {
        protected ResultContainer<TResult> Results { get; set; }

        public Command()
        {
        }

        public abstract Task SendRequest();
        public virtual ResultContainer<TResult> GetResults() => Results ?? null;
    }
}
