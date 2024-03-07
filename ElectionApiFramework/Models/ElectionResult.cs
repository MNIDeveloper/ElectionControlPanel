using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionApiFramework.Models
{
    public class ElectionResult
    {
        public int CandidateId { get; set; }
        public string PartyName { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public int Votes { get; set; }
        public decimal Percentage { get; set; }
    }
}
