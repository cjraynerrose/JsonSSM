using System;
using System.IO;
using System.Linq;
using JsonSSM.Results;

namespace JsonSSM
{
    public class CreateFileCommand
    {
        public ResultContainer<GetResult> GetResult { get; }
        public string OutputPath { get; }
        public FileCreateResult Result { get; private set; }

        public CreateFileCommand(ResultContainer<GetResult> getResult, string outputPath)
        {
            GetResult = getResult;
            OutputPath = outputPath;
        }

        public ResultContainer<FileCreateResult> Invoke()
        {
            throw new NotImplementedException();
            //GetResult.Results.FirstOrDefault().Response.
        }
    }
}