using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace GymApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _db.Products.ToList();
            return View(objProductList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        public async Task<IActionResult> Edit(string ProductId)
        {
            Product? productdb = await _db.Products.FindAsync(ProductId);
            
            if(productdb == null)
            {
                return NotFound();
            }

            return View(productdb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public async Task<IActionResult> Delete(string ProductId)
        {
            Product? productdb = await _db.Products.FindAsync(ProductId);

            if (productdb == null)
            {
                return NotFound();
            }

            return View(productdb);
        }
        [HttpPost]
        public IActionResult Delete(Product obj)
        {
            _db.Products.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Product");
            
        }
    }
}
