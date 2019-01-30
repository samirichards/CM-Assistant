using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assistant_UWP.Classes.Models
{
    class Transaction
    {
        public double Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        public Session InSource { get; set; }
        public Business OutSource { get; set; }
        public int ID { get; set; }
    }
}
