using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectionApiFramework.Models
{
    public class CandidateDFED
    {
        public string FullName { get; set; }
        public int CandidateId { get; set; }
        
        public int PersonId { get; set; }
        
        public int Party { get; set; }
        
        public int Constituancy { get; set; }
        public string CandidateImage { get; set; }
        public string CandidateImageWeb { get; set; }

        public HttpPostedFileBase ImageData { get; set; }
        public bool IsCurrent { get; set; } 
    }
}
