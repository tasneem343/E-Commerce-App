using System.ComponentModel.DataAnnotations;

namespace Ecommerce_App.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name = "Role NAme")]
        public string RoleName { get; set; }
    }
}
