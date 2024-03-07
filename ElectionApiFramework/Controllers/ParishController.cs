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
    
    public class ParishController : ApiController
    {
        private AdminBLL admin = new AdminBLL();

        public Boolean AddParish(Parish parish)
        {
            return admin.AddParish(parish);
        }
       
        public Boolean DeleteParish(int parishID)
        {
            return admin.DeleteParish(parishID);
        }
        
        public Boolean UpdateParish(Parish parish)
        {
            return admin.UpdateParish(parish);
        }
        
        public IHttpActionResult GetParishes()
        {
            var parishes = admin.GetParishes();
            if(parishes == null)
            {
                return NotFound();
            }
            return Ok(parishes);
        }
    }
}
