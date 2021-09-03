using System.Collections;
using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public abstract class FileWriter<T> : IFileWriter 
        where T : ITimedData
    {
        protected readonly FileScale FileScale;
        protected readonly string FilePath;
        protected readonly DateTimePart TimeFrameMajorScale;
        protected readonly uint TimeFrameMinorScale;
        protected readonly string TimeFrame;
        protected List<string> _filePaths;

        protected FileWriter() {}
        public FileWriter(LoaderConfig cfg)
        {
            FileScale = cfg.FileScale;
            FilePath = cfg.OutputFolder;
            TimeFrameMajorScale = cfg.TimeFrameMajorScale;
            TimeFrameMinorScale = cfg.TimeFrameMinorScale;
            TimeFrame = $"{TimeFrameMajorScale}{TimeFrameMinorScale}";
            _filePaths = new List<string>();
        }
        protected abstract bool Write(string product, QuoteSide side, IEnumerable<T> data);
        bool IFileWriter.Write(string product, QuoteSide side, IEnumerable data)
        {
            return Write(product, side, (IEnumerable<T>)data);
        }

        List<string> IFileWriter.FilePaths => _filePaths;
    }
}