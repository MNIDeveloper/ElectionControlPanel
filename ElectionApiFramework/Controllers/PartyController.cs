using ElectionApiFramework.BLL;
using ElectionApiFramework.Interfaces;
using ElectionApiFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ElectionApiFramework.Controllers
{
   
    public class PartyController :ApiController
    {
        private IAdministrative admin = new AdminBLL();

        public Boolean AddParty(Party party)
        {
            return admin.AddParty(party);
        }
        
        public Boolean DeleteParty(int PartyID)
        {
            return admin.DeleteParty(PartyID);
        }
       
        public Boolean UpdateParty(Party party)
        {
            return admin.UpdateParty(party);
        }
       
        public IHttpActionResult GetParties()
        {
            var parties = admin.GetParties();
            if(parties == null)
            {
                return NotFound();
            } 
            return Ok(parties);
        }
    }
}
