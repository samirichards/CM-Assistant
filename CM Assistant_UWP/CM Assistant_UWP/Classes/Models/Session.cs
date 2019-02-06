using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace CM_Assistant_UWP.Classes.Models
{
    class Session
    {
        [AutoIncrement, PrimaryKey]
        public int SessionID { get; set; }
        public int ChildID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
