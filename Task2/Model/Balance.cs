using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Model
{
    public class Balance
    {
        public int Id { get; set; }
        public int CountId { get; set; }
        public decimal InputActive { get; set; }
        public decimal InputPassive { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal OutputActive { get; set; }
        public decimal OutputPassive { get; set; }
    }
}
