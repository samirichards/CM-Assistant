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
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset ?End { get; set; }
        public bool SessionOpen { get; set; }
    }
}
