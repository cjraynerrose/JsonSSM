using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonSSM.Parameters.Put
{
    public class FileLoader
    {
        public JToken Get(string filePath)
        {
            using var reader = new StreamReader(filePath);
            var data = reader.ReadToEnd();
            JToken json;
            try
            {
                json = JToken.Parse(data);

            }
            catch (JsonReaderException e)
            {

                throw e;
            }

            return json;
        }
    }
}
