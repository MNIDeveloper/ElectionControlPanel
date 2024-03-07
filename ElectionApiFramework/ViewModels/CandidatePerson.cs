using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectionApiFramework.ViewModels
{
    public class CandidatePerson
    {
        public string FullName { get; set; }
        public int CandidateId { get; set; }
        [Required(ErrorMessage = "Person ID is Required")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "A Party is Required")]
        public int Party { get; set; }
        [Required(ErrorMessage = "A Constituency is Required")]
        public int Constituancy { get; set; }
        public string CandidateImage { get; set; }
        public string CandidateImageWeb { get; set; }
        public bool IsCurrent { get; set; }
        public HttpPostedFileBase ImageData { get; set; }

        public List<Person> VoterList { get; set; } = new List<Person>();
        public List<Constituancy> constituencies { get; set; }
        public List<Party> parties { get; set; } = new List<Party>();
    }
}
