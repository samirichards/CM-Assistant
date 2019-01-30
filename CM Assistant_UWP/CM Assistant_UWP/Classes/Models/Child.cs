using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assistant_UWP.Classes.Models
{
    class Child
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Rate { get; set; }
        public double AltRate { get; set; }
        public string DietNotes { get; set; }
        public string MedicalNotes { get; set; }
        public string Notes { get; set; }
        public List<LearningNote> LearningNotes { get; set; }
        public Times Timetable { get; set; }
    }
}
