using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace JsonSSM.Models.Data
{
    public class DataContainer
    {
        Parameter Parameter { get; set; }
        object Value { get; }
        string Path { get; }

        public DataContainer(string path, object value)
        {
            Path = EnsureFormatting(path);
            Value = value;

            InitializeParameter();
        }

        private void InitializeParameter()
        {
            Parameter = new Parameter
            {
                Name = Path,
                Value = Value.ToString()
            };
        }

        private string EnsureFormatting(string value)
        {
            if (!value.StartsWith('/'))
            {
                value = $"/{value}";
            }

            return value;
        }

        public void SetParameterType(ParameterType parameterType) => Parameter.Type = parameterType;
        public string GetPath() => Path;
        public bool HasPath(string value) => Path == value;
        public object GetValue() => Value;
        public bool HasValue(object value) => Value == value;
        public string GetParameterName() => Parameter.Name;
        public string GetParameterValue() => Parameter.Value;
        public ParameterType GetParameterType() => Parameter.Type;

        public string GetParameterDataType() => Parameter.DataType;

    }
}
