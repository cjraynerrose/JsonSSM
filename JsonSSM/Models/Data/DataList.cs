using System.Collections.Generic;
using System.Linq;

using Amazon.SimpleSystemsManagement;

namespace JsonSSM.Models.Data
{
    public class DataList
    {
        public MetaData Meta { get; }
        public List<DataContainer> Data { get; }

        public DataList(MetaData meta, List<DataContainer> list)
        {
            Meta = meta;
            Data = list;
        }

        public object FindValueByPath(string path)
        {
            var value = Data.FirstOrDefault(x => x.GetPath() == path)?.GetValue();
            return value;
        }

        public ParameterType FindParameterTypeByName(string name)
        {
            var value = Data.FirstOrDefault(x => x.GetParameterName() == name).GetParameterType();
            return value;
        }
    }
}
