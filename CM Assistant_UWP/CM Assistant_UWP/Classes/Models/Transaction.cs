using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace CM_Assistant_UWP.Classes.Models
{
    class Transaction
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }

        public double Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        public int InSourceID { get; set; }
        public int OutSourceID { get; set; }
    }
}
