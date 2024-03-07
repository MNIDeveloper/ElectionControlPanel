using ElectionApiFramework.Interfaces;
using ElectionApiFramework.BLL;
using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace ElectionApiFramework.Controllers
{
   
    public class ElectionController :ApiController
    {
        private ElectionBLL _election = new ElectionBLL();


        // GET api/Election        
        public Person Get()
        {
            var person = new Person();
            return person; 
        }
        // GET api/Election?VotersID=''&Pin=''       
        public Boolean Get(int VotersID, string Pin)
        {
            return _election.Login(VotersID, Pin);
        }
        // GET api/Election?VotersID=''        
        public IHttpActionResult Get(int VotersID)
        {
            var perosn = _election.GetPerson(VotersID); 
            if (perosn.PersonId == 0) 
            {
                return NotFound(); 
            }
            return Ok(perosn);
        }
        
        public Boolean Post([FromBody] Election election)
        {
            return _election.AddVotes(election);
        }
        public Boolean Get(int personID,bool vFlag)
        {
            return _election.LockOutVoter(personID);
        }
    }
}
