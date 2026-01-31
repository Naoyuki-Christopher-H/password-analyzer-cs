using Microsoft.AspNetCore.Mvc;

namespace password_analyzer_cs.Controllers
{
    public class PasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
