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
    
    public class AdminController : ApiController
    {
        private IAdministrative admin = new AdminBLL();
        public Boolean UserLogin(string Username, string Password)
        {
            return admin.UserLogin(Username, Password);
        }

       
        public IHttpActionResult GetUser(string Username)
        {
            var person = admin.GetUser(Username);
            if (person.UserId == 0)
            {
                return NotFound();
            }
            return Ok(person);
        }
        
        public Boolean AddUser(User user)
        {
            return admin.AddUser(user);
        }
        
        public Boolean DeleteUser(int userID)
        {
            return admin.DeleteUser(userID);
        }
        
        public Boolean UpdateUser(User user)
        {
            return admin.UpdateUser(user);
        }
    }
}
