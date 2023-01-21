namespace Task2.Model.Files
{
    public class FileDetails
    {
        public int Id { get; set; }
        //public int BalanceId { get; set; }
        //public Balance Balance { get; set; }
        //public CustomString CustomString { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public FileType FileType { get; set; }
    }
}
