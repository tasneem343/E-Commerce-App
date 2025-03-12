using System.ComponentModel.DataAnnotations;

namespace Ecommerce_App.ViewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
