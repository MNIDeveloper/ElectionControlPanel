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
    
    public class ConstituencyController : ApiController
    {
        private IAdministrative admin = new AdminBLL();
       
        public Boolean AddConstituancy(Constituancy constituancy)
        {
            return admin.AddConstituancy(constituancy);
        }
        
        public Boolean DeleteConstituancy(int ConstituancyID)
        {
            return admin.DeleteConstituancy(ConstituancyID);
        }
        
        public Boolean UpdateConstituancy(Constituancy constituancy)
        {
            return admin.UpdateConstituancy(constituancy);
        }
        
        public IHttpActionResult GetConstituancies()
        {
            var constituancies = admin.GetConstituancies();
            if(constituancies == null)
            {
                return NotFound();
            } 
            return Ok(constituancies);
        }
    }
}
