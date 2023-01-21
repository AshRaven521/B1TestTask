using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task2.Model.Files;

namespace Task2.Model
{
    public class Balance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public FileDetails File { get; set; }
        public int FileId { get; set; }
        public int ExcelRowNumber { get; set; }
        public int CountId { get; set; } 
        public decimal InputActive { get; set; }
        public decimal InputPassive { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal OutputActive { get; set; }
        public decimal OutputPassive { get; set; }
    }
}
