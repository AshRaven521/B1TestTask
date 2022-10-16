namespace Task1.Utils
{
    public class Callbacks
    {
        public delegate void joinFilesCallback(int result);

        public delegate void generateFilesCallback();

        public delegate void insertIntoDB(int lines);
    }
}
