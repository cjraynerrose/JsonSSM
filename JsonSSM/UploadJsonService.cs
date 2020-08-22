using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using JsonSSM.Clients;
using JsonSSM.Files;
using JsonSSM.Mappers;
using JsonSSM.Models.Results;

namespace JsonSSM
{
    public class UploadJsonService
    {
        public async Task<ResultContainer> Upload(string path)
        {
            FileLoader fl = new FileLoader();
            var json = fl.Get(path);

            JsonFlattener jf = new JsonFlattener();
            var dataList = jf.Flatten(json);

            ParameterUploadClient puc = new ParameterUploadClient(dataList);
            await puc.Upload();

            ResultContainer result = puc.GetResults();
            return result;
        }
    }
}
