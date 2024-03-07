using System.ComponentModel.DataAnnotations;

namespace ElectionApiFramework.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string Fname { get; set; }
        [Required(ErrorMessage = " Last Name is required")]
        public string Lname { get; set; }
        [Required(ErrorMessage = "Position is required")]
        public int? Position { get; set; }
    }
}
