using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SimpleSystemsManagement;
using JsonSSM.Results;

namespace JsonSSM.Parameters
{
    public abstract class ParameterQuery<TResult> : Query<TResult> where TResult : Result
    {
        protected AmazonSimpleSystemsManagementClient SSMClient { get; set; }

        public ParameterQuery(string regionName)
        {
            var region = RegionEndpoint.GetBySystemName(regionName);
            SSMClient = new AmazonSimpleSystemsManagementClient(region);
        }
    }
}
