namespace Task1.Utils
{
    /// <summary>
    /// Class that contains needed callbacks
    /// </summary>
    public class Callbacks
    {
        /// <summary>
        /// Delegate for function that join files in one result file
        /// </summary>
        /// <param name="result"></param>
        public delegate void joinFilesCallback(int result);

        /// <summary>
        /// Delegate for function that generate files
        /// </summary>
        public delegate void generateFilesCallback();


        /// <summary>
        /// Delegate for function that insert data into database
        /// </summary>
        /// <param name="lines"></param>
        public delegate void insertIntoDB(int lines);
    }
}
