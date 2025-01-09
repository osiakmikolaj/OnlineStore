using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        // Product list
        private static IList<Product> products = new List<Product>
        {
            new Product() { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 3500 },
            new Product() { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 2600 },
            new Product() { Id = 3, Name = "Tablet", Description = "Lightweight tablet", Price = 1400 }
        };

        // GET: /Product/
        public ActionResult Index()
        {
            return View(products);
        }

        // GET: /Product/Details/
        public ActionResult Details(int id)
        {
            return View(products.FirstOrDefault(x => x.Id == id));
        }

        // GET: /Product/Create/
        public ActionResult Create()
        {
            return View(new Product());
        }

        // POST: /Product/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                product.Id = products.Max(x => x.Id) + 1;
                products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            { 
                return View();
            }
        }

        // GET: /Product/Edit/
        public ActionResult Edit(int id)
        {
            return View(products.FirstOrDefault(x => x.Id == id));
        }

        // POST: /Product/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                product = products.FirstOrDefault(x => x.Id == id);
                product.Name = product.Name;
                product.Description = product.Description;
                product.Price = product.Price;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: /Product/Delete/
        public ActionResult Delete(int id)
        {
            return View(products.FirstOrDefault(x => x.Id == id));
        }

        // POST: /Product/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            { 
                product = products.FirstOrDefault(x => x.Id == id);
                products.Remove(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}