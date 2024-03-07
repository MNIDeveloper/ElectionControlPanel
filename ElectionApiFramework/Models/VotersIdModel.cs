using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionApiFramework.Models
{
    public class VotersIdModel
    {
        public string FullName { get; set; }    
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Constituency { get; set; }    
        public string VotersID { get; set; }
        public string Pin { get; set; }
        public string VoterImage { get; set; }
        public string VoterBarcode { get; set;}
        public bool MissingData { get; set; }
    }
}
