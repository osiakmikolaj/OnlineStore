using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Identity.Data;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OrderController
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
            return View(orders);
        }

        // GET: OrderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product).FirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: OrderController/Create
        public IActionResult Create()
        {
            return View(new Order());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                order.UserId = userId;
                order.TotalPrice = CalculateTotalPrice(order);
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: OrderController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.OrderProducts.RemoveRange(order.OrderProducts);

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        private int CalculateTotalPrice(Order order)
        {
            return (int)(order.OrderProducts?.Sum(op => op.Product.Price * op.Quantity) ?? 0);
        }
    }
}
