using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace CM_Assistant_UWP.Classes.Models
{
    class Client : Entity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
    }
}
