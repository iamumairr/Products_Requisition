#nullable disable
using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RequestController(ApplicationDbContext context)
        {
            _context= context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int? id)
        {
            if (id != null)
            {

            }
            return View();
        }
    }
}
