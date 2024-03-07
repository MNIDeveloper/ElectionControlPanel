using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionApiFramework.Models;

namespace ElectionApiFramework.ViewModels
{
    public class UserVM
    {
        public List<User> users = new List<User>();
        public User EditUser { get; set; }  = new User();
        public User DeleteUser { get; set; } = new User();
        public User AddUser { get; set; } = new User();
        public int modalopen { get; set; } = 0;

    }
}
