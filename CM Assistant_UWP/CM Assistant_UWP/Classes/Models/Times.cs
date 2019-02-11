using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace CM_Assistant_UWP.Classes.Models
{
    class Times
    {
        [PrimaryKey, AutoIncrement]
        public int TimesID { get; set; }

        public DateTime StartTime_Mon { get; set; }
        public DateTime EndTime_Mon { get; set; }

        public DateTime StartTime_Tue { get; set; }
        public DateTime EndTime_Tue { get; set; }

        public DateTime StartTime_Wed { get; set; }
        public DateTime EndTime_Wed { get; set; }

        public DateTime StartTime_Thur { get; set; }
        public DateTime EndTime_Thur { get; set; }

        public DateTime StartTime_Fri { get; set; }
        public DateTime EndTime_Fri { get; set; }

        public DateTime StartTime_Sat { get; set; }
        public DateTime EndTime_Sat { get; set; }

        public DateTime StartTime_Sun { get; set; }
        public DateTime EndTime_Sun { get; set; }
    }
}
