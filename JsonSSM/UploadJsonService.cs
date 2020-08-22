using System.Threading.Tasks;
using JsonSSM.Parameters.Put;
using JsonSSM.Results;

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

            PutClient puc = new PutClient(dataList);
            await puc.Upload();

            ResultContainer result = puc.GetResults();
            return result;
        }
    }
}
