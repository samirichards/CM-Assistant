using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using SQLite;
using SQLitePCL;

namespace CM_Assistant_UWP.Classes.Models
{
    class Child : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Rate { get; set; }
        public double AltRate { get; set; }
        public string DietNotes { get; set; }
        public string MedicalNotes { get; set; }
        public string Notes { get; set; }
        public int ParentID { get; set; }
        public byte[] Photo { get; set; }
    }
}
