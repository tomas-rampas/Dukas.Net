using System.Collections;
using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    interface IFileWriter
    {
        internal  bool Write(IEnumerable data);
    }
    
    public abstract class FileWriter<T> : IFileWriter 
        where T : ITimedData
    {
        protected readonly LoaderConfig _cfg;
        protected readonly DateTimePart fileScale = DateTimePart.Day;
        protected FileWriter() {}
        
        public FileWriter(LoaderConfig cfg)
        {
            _cfg = cfg;
        }

        internal abstract bool Write(IEnumerable<T> data);


        bool IFileWriter.Write(IEnumerable data)
        {
            return Write((IEnumerable<T>)data);
        }
    }
}