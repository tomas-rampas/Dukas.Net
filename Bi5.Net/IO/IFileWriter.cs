using System.Collections;

namespace Bi5.Net.IO
{
    internal interface IFileWriter
    {
        /// <summary>
        /// Writes data to file
        /// </summary>
        /// <param name="product"></param>
        /// <param name="data">List of prices</param>
        /// <returns>True if bool ended up successfully</returns>
        internal bool Write(string product, IEnumerable data);
    }
}