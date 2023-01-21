using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.Model
{
    public class CustomString
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string Content { get; set; }
        public int ExcelRowNumber { get; set; }
    }
}
