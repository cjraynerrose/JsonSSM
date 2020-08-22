using System.Threading.Tasks;
using JsonSSM.Parameters.Put;
using JsonSSM.Results;

namespace JsonSSM
{
    public class UploadJsonService
    {
        public async Task<ResultContainer<PutResult>> Upload(string path)
        {
            FileLoader fl = new FileLoader();
            var json = fl.Get(path);

            JsonFlattener jf = new JsonFlattener();
            var dataList = jf.Flatten(json);

            PutCommand command = new PutCommand(dataList);
            await command.SendRequest();

            ResultContainer<PutResult> result = command.GetResults();
            return result;
        }
    }
}
