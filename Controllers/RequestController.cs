using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
