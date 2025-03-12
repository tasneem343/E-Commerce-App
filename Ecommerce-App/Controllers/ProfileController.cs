using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult showProfile()
        {
            return View();
        }
    }
}
