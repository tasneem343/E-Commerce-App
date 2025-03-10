using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_App.ViewModels
{
    public class RegisterViewModel
    {
        [Length(3, 20)]
        public string UserName { get; set; }
        [Length(3, 20)]

        public string FirstName { get; set; }
        [Length(3, 20)]

        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        
        public string UserType { get; set; }
    }
}
