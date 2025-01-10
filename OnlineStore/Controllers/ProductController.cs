using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineStore.Areas.Identity.Data;
using OnlineStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(AplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Product/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: /Product/Details/
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: /Product/Create/
        public IActionResult Create()
        {
            return View(new Product());
        }

        // POST: /Product/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: /Product/Edit/
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Product/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: /Product/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> AddToOrder(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var order = GetOrderFromSession();
            var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == productId);
            if (orderProduct != null)
            {
                orderProduct.Quantity += quantity;
            }
            else
            {
                order.OrderProducts.Add(new OrderProduct { ProductId = productId, Quantity = quantity, Product = product });
            }
            SaveOrderToSession(order);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var order = GetOrderFromSession();
            if (order.OrderProducts.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            order.UserId = userId;
            order.CustomerName = $"{user.FirstName} {user.LastName}";
            order.CustomerEmail = user.Email;
            order.TotalPrice = (int)CalculateTotalPrice(order);

            // Powiąż istniejące produkty z zamówieniem
            foreach (var orderProduct in order.OrderProducts)
            {
                _context.Entry(orderProduct.Product).State = EntityState.Unchanged;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            ClearOrderFromSession();

            return RedirectToAction("Details", "Order", new { id = order.OrderId });
        }


        private Order GetOrderFromSession()
        {
            var orderJson = HttpContext.Session.GetString("Order");
            if (string.IsNullOrEmpty(orderJson))
            {
                return new Order { OrderProducts = new List<OrderProduct>() };
            }
            return JsonConvert.DeserializeObject<Order>(orderJson);
        }

        private void SaveOrderToSession(Order order)
        {
            var orderJson = JsonConvert.SerializeObject(order);
            HttpContext.Session.SetString("Order", orderJson);
        }

        private void ClearOrderFromSession()
        {
            HttpContext.Session.Remove("Order");
        }

        private decimal CalculateTotalPrice(Order order)
        {
            return order.OrderProducts?.Sum(op => op.Product.Price * op.Quantity) ?? 0;
        }

        [HttpPost]
        public IActionResult RemoveFromOrder(int productId)
        {
            var order = GetOrderFromSession();
            var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == productId);
            if (orderProduct != null)
            {
                order.OrderProducts.Remove(orderProduct);
                SaveOrderToSession(order);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}