namespace Bi5.Net.Models
{
    /// <summary>
    /// Defines frequency of storing the data.
    /// E.g. when FileScale is FileScale.Day data will be grouped by Day and store in the file with frequency
    /// where one file represents one day of data 
    /// </summary>
    public enum FileScale
    {
        Min,
        /// <summary>
        /// Hour
        /// </summary>
        Hour,
        /// <summary>
        /// Day
        /// </summary>
        Day,
        /// <summary>
        /// Week. Currently not supported.
        /// </summary>
        Week,
        /// <summary>
        /// Month
        /// </summary>
        Month,
        /// <summary>
        /// Year
        /// </summary>
        Year,
        /// <summary>
        /// All data in one file
        /// </summary>
        Full
        
    }
}