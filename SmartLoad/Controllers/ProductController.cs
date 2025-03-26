using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Data;
using SmartLoad.Models;
using SmartLoad.Services;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLoad.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly HomeController _loadingService;
        public ProductController(ApplicationDbContext context, LoadingService loadingService)
        {
            _context = context;
            //_loadingService = loadingService;
        }

        #region Продукт

        public IActionResult Products()
        {
            var products = GetProducts();
            return View(products);
        }

        private List<Product> GetProducts()
        {
            return _context.Products
                .Include(p => p.PackagingTypes)
            .ToList();
        }

        public IActionResult AddProduct()
        {
            ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении продукта: {ex.Message}");
                return View(product);
            }
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }
            return View(product);
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var product = await _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }

        #endregion
    }
}
