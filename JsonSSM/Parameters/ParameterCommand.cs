using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using JsonSSM.Models;
using JsonSSM.Results;

namespace JsonSSM.Parameters
{
    public abstract class ParameterCommand<TResult> : Command<TResult> where TResult : Result
    {
        protected DataList DataList { get; }
        protected AmazonSimpleSystemsManagementClient SSMClient { get; set; }

        public ParameterCommand(DataList dataList)
            : base()
        {
            DataList = dataList;
            SSMClient = new AmazonSimpleSystemsManagementClient(DataList.Meta.Region);
        }
    }
}
