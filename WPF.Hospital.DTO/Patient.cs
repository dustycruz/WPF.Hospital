using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Hospital.DTO
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }

        // ✅ CHANGE: initialize collection
        public List<History> History { get; set; } = new();
    }
}
