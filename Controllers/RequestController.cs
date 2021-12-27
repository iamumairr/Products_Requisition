#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var requests=new List<Request>();

            if (User.IsInRole(WC.AdminRole))
            {
                requests = _context.Requests.Include(a=>a.Product).Include(a => a.ApplicationUser).ToList();
            }
            else if(User.IsInRole(WC.CoordinatorRole))
            {
                requests = _context.Requests.Where(a => a.ApplicationUser.Id != _userManager.GetUserId(User)).Include(a => a.Product).Include(a=>a.ApplicationUser).ToList();
            }
            else
            {
                requests = _context.Requests.Where(a => a.ApplicationUser.Id == _userManager.GetUserId(User)).Include(a => a.Product).Include(a => a.ApplicationUser).ToList();
            }

            return View(requests);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.FirstOrDefault(a => a.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Request request = new Request()
            {
                Product=product,
                ProductId=product.ProductId,
                ApplicationUser =await _userManager.GetUserAsync(HttpContext.User),
                ApplicationUserId =_userManager.GetUserId(User)
            };
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.Request rvm)
        {
            if (ModelState.IsValid)
            {
                Request req = new Request();
                req.ProductId = rvm.ProductId;
                req.RequestDate = DateTime.Now;
                req.QuantityRequest = rvm.QuantityRequest;
                req.ApplicationUserId = _userManager.GetUserId(User);
                req.Justification = rvm.Justification;
                req.Note = rvm.Note;

                Product product = _context.Products.FirstOrDefault(a => a.ProductId == rvm.ProductId);

                req.TotalAmount = rvm.QuantityRequest * product.UnitaryAmount;

                var stock = product.StockQuantity;
                if (rvm.QuantityRequest > stock)
                {
                    req.RequestStatus = Status.PENDING;
                }
                else
                {
                    if (product.Level == Level.Zero)
                    {
                        req.RequestStatus = Status.APPROVED;

                        product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                    }
                    else if (product.Level == Level.One)
                    {
                        if (User.IsInRole(WC.CoordinatorRole))
                        {
                            req.RequestStatus = Status.APPROVED;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                        else
                        {
                            req.RequestStatus = Status.PENDING;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                    }
                    else
                    {
                        if (User.IsInRole(WC.CoordinatorRole))
                        {
                            req.RequestStatus = Status.PARTIAL;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                        else
                        {
                            req.RequestStatus = Status.PENDING;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                    }
                }
                _context.Requests.Add(req);
                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            var prod = _context.Products.FirstOrDefault(a => a.ProductId == rvm.ProductId);
            
            Request request = new Request()
            {
                ProductId = prod.ProductId
            };
            return View(request);
        }
        [HttpGet]
        [Authorize(Roles ="Admin,Coordinator")]
        public IActionResult UpdateStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = _context.Requests.FirstOrDefault(a=>a.RequestId==id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id,Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var objFromDB = _context.Requests.FirstOrDefault(a => a.RequestId == id);
                if (objFromDB == null)
                {
                    return NotFound();
                }
                objFromDB.RequestStatus = request.RequestStatus;

                _context.Update(objFromDB);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
