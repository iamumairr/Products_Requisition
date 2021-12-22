using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
