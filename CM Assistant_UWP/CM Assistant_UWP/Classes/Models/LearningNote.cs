using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace CM_Assistant_UWP.Classes.Models
{
    class LearningNote
    {
        public int ChildID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Contents { get; set; }
    }
}
