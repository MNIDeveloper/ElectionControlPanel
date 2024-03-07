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
    
    public class AddressController : ApiController
    {
        private IAdministrative admin = new AdminBLL();

        
        public Boolean AddAddress(Address address)
        {
            return admin.AddAddress(address);
        }
       
        public Boolean DeleteAddress(int AddressID)
        {
            return admin.DeleteAddress(AddressID);
        }
        
        public Boolean UpdateAddress(Address address)
        {
            return admin.UpdateAddress(address);
        }
        
        public IHttpActionResult GetAddresses(Person person)
        {
            var addresses = admin.GetAddresses(person);
            if(addresses == null)
            {
                return NotFound();
            } 
            return Ok(addresses);
        }
        public IHttpActionResult GetAddressByID(int AddressID)
        {
            var address = admin.GetAddressById(AddressID);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }
    }
}
