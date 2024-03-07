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
    public class VoterPerson
    {
        public int PersonId { get; set; }

        [Required(ErrorMessage ="A First Name is Required")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        [Required(ErrorMessage ="A Surname is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="A Date of Birth is Required")]
        
        public DateTime Dob { get; set; }

        public string Alias { get; set; }

        public int Address { get; set; }

        public DateTime RegDate { get; set; }

        public bool VFlag { get; set; }
        [Required(ErrorMessage ="A Pin Number is Required for Voter Login")]
        public string Pin { get; set; }
        [Required(ErrorMessage ="A Social Security Number is Required for External ID")]
        public int? SocialSecurity { get; set; }
        [Required(ErrorMessage = "A Gender Selection is Required ")]
        public string Gender { get; set; }
        //[Required(ErrorMessage = "Required to create Voter's ID Document")]
        public HttpPostedFileBase ImageData { get; set; }
        public string PersonImage { get; set; }


        public Boolean AddressIsCurrent { get; set; }
        public Boolean AddressFlag { get; set; }
        public Address addressObject { get; set; }

        public List<Person> VoterList { get; set; } = new List<Person>();
        public List<Village> villages { get; set; }
        public List<Parish> parishes { get; set; }
        public List<Constituancy> constituencies { get; set; }
        public List<Address> VoterAddresses { get; set; }
    }
}
