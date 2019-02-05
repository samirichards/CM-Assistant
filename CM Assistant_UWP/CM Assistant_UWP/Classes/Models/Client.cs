using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assistant_UWP.Classes.Models
{
    class Client : Entity
    {
        public List<Child> Children { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
    }
}
