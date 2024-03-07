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
   
    public class VillageController : ApiController
    {
        private IAdministrative admin = new AdminBLL();

        public Boolean AddVillage(Village village)
        {
            return admin.AddVillage(village);
        }
        
        public Boolean DeleteVillage(int VillageID)
        {
            return admin.DeleteVillage(VillageID);
        }
        
        public Boolean UpdateVillage(Village village)
        {
            return admin.UpdateVillage(village);
        }
        
        public IHttpActionResult GetVillages()
        {
            var villages = admin.GetVillages();
           if(villages == null)
            {
                return NotFound();
            }
            return Ok(villages);
        }
    }
}
