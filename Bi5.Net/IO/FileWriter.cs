using System.Collections;
using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public abstract class FileWriter<T> : IFileWriter 
        where T : ITimedData
    {
        protected readonly FileScale _fileScale;
        protected readonly string _filePath;

        protected FileWriter() {}
        public FileWriter(LoaderConfig cfg)
        {
            _fileScale = cfg.FileScale;
            _filePath = cfg.OutputFolder;
        }
        protected abstract bool Write(string product, IEnumerable<T> data);
        bool IFileWriter.Write(string product, IEnumerable data)
        {
            return Write(product, (IEnumerable<T>)data);
        }
    }
}