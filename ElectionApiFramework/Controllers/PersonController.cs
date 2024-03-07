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
    
    public class PersonController : ApiController
    {
        private AdminBLL admin = new AdminBLL();

        public Boolean AddPerson(Person person)
        {
            return admin.AddPerson(person);
        }
        
        public Boolean DeleteConstituancy(int PersonID)
        {
            return admin.DeletePerson(PersonID);
        }
       
        public Boolean UpdatePerson(Person person)
        {
            return admin.UpdatePerson(person);
        }
        
        public IHttpActionResult GetPerson(int PersonID)
        {
            var people = admin.GetPersonById(PersonID);
           if(people == null)
            {
                return NotFound();
            }
            return Ok(people);
        }
    }
}
