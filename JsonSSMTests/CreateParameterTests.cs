using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using JsonSSM;
using JsonSSM.Models;
using JsonSSM.Parameters.Delete;
using JsonSSM.Parameters.Put;
using JsonSSM.Results;
using NUnit.Framework;

namespace JsonSSMTests
{
    public class CreateParameterTests
    {
        private FileLoader FileLoader;
        private JsonFlattener JsonFlattener;

        [SetUp]
        public void Setup()
        {
            FileLoader = new FileLoader();
            JsonFlattener = new JsonFlattener();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            var files = new string[]
            {
                "create_test_input/test_upload_to_s3.json",
                "create_test_input/test_upload_service.json",
                "create_test_input/test_encrypt_meta.json"
            };

            var deletionClients = files.Select(f =>
                new DeleteClient(
                    JsonFlattener.Flatten(
                        FileLoader.Get(f)
                        )
                    )
                );


            foreach (var client in deletionClients)
            {
                await client.Delete();
            }
        }

        [TestCase("create_test_input/test_exists.json")]
        public void GetFileTest(string path)
        {
            var json = FileLoader.Get(path);
            Assert.NotNull(json);
            Assert.True(json.Value<bool>("Exists"));
        }

        [TestCase("create_test_input/test_get_all_paths.json")]
        public void GetAllPathsTest(string path)
        {
            DataList dataList = GetDataFromFile(path);

            Assert.AreEqual("value1", dataList.FindValueByPath("/RootString"));
            Assert.AreEqual("Some Kind Of Value", dataList.FindValueByPath("/Path/SubPath/String"));
            Assert.AreEqual(42, dataList.FindValueByPath("/Path/SubPath/Int"));
            Assert.AreEqual(true, dataList.FindValueByPath("/Path/SubPath/Bool"));
        }

        [TestCase("create_test_input/test_pv_to_objects.json")]
        [TestCase("create_test_input/test_pv_to_objects_2.json")]
        public void ConvertPathValuesToObjectsTest(string path)
        {
            DataList dataList = GetDataFromFile(path);

            var stringActual = dataList.Data.ElementAt(0);
            var stringListActual = dataList.Data.ElementAt(1);

            var stringExpected = new Parameter
            {
                Name = "/String",
                Type = ParameterType.String,
                Value = "value"
            };

            var stringListExpected = new Parameter
            {
                Name = "/StringList",
                Type = ParameterType.StringList,
                Value = "value1, value2, value3"
            };

            Assert.AreEqual(stringExpected.Name, stringActual.GetParameterName());
            Assert.AreEqual(stringExpected.Type, stringActual.GetParameterType());
            Assert.AreEqual(stringExpected.Value, stringActual.GetParameterValue());

            Assert.AreEqual(stringListExpected.Name, stringListActual.GetParameterName());
            Assert.AreEqual(stringListExpected.Type, stringListActual.GetParameterType());
            Assert.AreEqual(stringListExpected.Value, stringListActual.GetParameterValue());
        }

        [TestCase("create_test_input/test_meta_data.json")]
        public void MetaDataResolverTest(string path)
        {
            DataList dataList = GetDataFromFile(path);

            Assert.IsNull(dataList.FindValueByPath("/META/region"));
            Assert.IsNull(dataList.FindValueByPath("/DATA/String"));
            Assert.AreEqual(RegionEndpoint.EUWest1, dataList.Meta.Region);
            Assert.IsTrue(dataList.Meta.Encrypt.Any(s => s == "/String"));
        }


        [TestCase("create_test_input/test_encrypt_meta.json")]
        [TestCase("create_test_input/test_upload_to_s3.json")]
        public void EncryptPathsTest(string path)
        {
            DataList dataList = GetDataFromFile(path);

            Assert.AreEqual(ParameterType.SecureString, dataList.FindParameterTypeByName("/Encrypted"));
            Assert.AreEqual(ParameterType.SecureString, dataList.FindParameterTypeByName("/EncryptedList/Enc"));
            Assert.AreEqual(ParameterType.String, dataList.FindParameterTypeByName("/EncryptedList/Plain"));
            Assert.AreEqual(ParameterType.SecureString, dataList.FindParameterTypeByName("/OtherList/a"));
            Assert.AreEqual(ParameterType.SecureString, dataList.FindParameterTypeByName("/OtherList/b"));
        }

        [TestCase("create_test_input/test_upload_to_s3.json")]
        public async Task UploadToS3Test(string path)
        {
            DataList dataList = GetDataFromFile(path);
            var uploadClient = new PutClient(dataList);

            Assert.IsNull(uploadClient.GetResults());

            await uploadClient.Upload();
            var result = uploadClient.GetResults();

            Assert.AreEqual(ResultType.Success, result.Type);
        }

        [TestCase("create_test_input/test_upload_service.json")]
        [TestCase("create_test_input/test_encrypt_meta.json")]
        [TestCase("create_test_input/test_upload_to_s3.json")]
        public async Task UploadServiceTest(string path)
        {
            var uploadService = new UploadJsonService();
            ResultContainer result = await uploadService.Upload(path);

            Assert.AreEqual(ResultType.Success, result.Type);
        }

        private DataList GetDataFromFile(string path)
        {
            var json = FileLoader.Get(path);
            var dataList = JsonFlattener.Flatten(json);
            return dataList;
        }
    }
}