using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class OrderController : Controller
    {
        private static IList<Order> orders = new List<Order>();

        // GET: OrderController
        public ActionResult Index()
        {
            return View(orders);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View(orders.FirstOrDefault(x => x.OrderId == id));
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View(new Order());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            try
            {
                order.OrderId = orders.Max(x => x.OrderId) + 1;
                orders.Add(order);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(orders.FirstOrDefault(x => x.OrderId == id));
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            try
            {
                order = orders.FirstOrDefault(x => x.OrderId == id);
                order.CustomerName = order.CustomerName;
                order.CustomerEmail = order.CustomerEmail;
                order.Products = order.Products;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
