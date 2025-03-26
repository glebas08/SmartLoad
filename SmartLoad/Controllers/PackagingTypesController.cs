using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;
using SmartLoad.Data;

namespace SmartLoad.Controllers
{
    public class PackagingTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackagingTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Тип упаковки

        public IActionResult PackagingTypes()
        {
            var packagingTypes = _context.PackagingTypes.ToList();
            return View(packagingTypes);
        }

        public IActionResult ProductPackaging(int productId)
        {
            var product = _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.Name;

            return View(product.PackagingTypes);
        }

        public IActionResult AddPackagingToProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.Name;

            return View();
        }

        [HttpPost]
        public IActionResult AddPackagingToProduct(PackagingType packagingType, int productId)
        {
            if (!ModelState.IsValid)
            {
                packagingType.ProductId = productId;
                _context.PackagingTypes.Add(packagingType);
                _context.SaveChanges();
                return RedirectToAction("ProductPackaging", new { productId });
            }

            ViewBag.ProductId = productId;
            ViewBag.ProductName = _context.Products.Find(productId)?.Name;
            return View(packagingType);
        }


        public IActionResult AddPackagingType()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddPackagingType(PackagingType packagingType)
        {
            if (!ModelState.IsValid)
            {
                // Проверка наличия продукта с указанным ProductId
                var product = _context.Products.Find(packagingType.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Выбранный продукт не существует.");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", packagingType.ProductId);
                    return View(packagingType);
                }

                _context.PackagingTypes.Add(packagingType);
                _context.SaveChanges();
                return RedirectToAction("PackagingTypes");
            }

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", packagingType.ProductId);
            return View(packagingType);
        }

        public IActionResult EditPackagingType(int id)
        {
            var packagingType = _context.PackagingTypes
                .Include(pt => pt.Product)
                .FirstOrDefault(pt => pt.Id == id);

            if (packagingType == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = packagingType.ProductId;
            ViewBag.ProductName = packagingType.Product.Name;

            return View(packagingType);
        }

        [HttpPost]
        public IActionResult EditPackagingType(PackagingType packagingType)
        {
            if (!ModelState.IsValid)
            {
                _context.PackagingTypes.Update(packagingType);
                _context.SaveChanges();
                return RedirectToAction("ProductPackaging", new { productId = packagingType.ProductId });
            }

            ViewBag.ProductId = packagingType.ProductId;
            ViewBag.ProductName = _context.Products.Find(packagingType.ProductId)?.Name;

            return View(packagingType);
        }

        public IActionResult DeletePackagingType(int id)
        {
            var packagingType = _context.PackagingTypes
                .Include(pt => pt.Product)
                .FirstOrDefault(pt => pt.Id == id);

            if (packagingType == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = packagingType.ProductId;
            ViewBag.ProductName = packagingType.Product.Name;

            return View(packagingType);
        }

        [HttpPost, ActionName("DeletePackagingType")]
        public async Task<IActionResult> DeletePackagingTypeConfirmed(int id)
        {
            var packagingType = await _context.PackagingTypes.FindAsync(id);
            if (packagingType == null)
            {
                return NotFound();
            }

            _context.PackagingTypes.Remove(packagingType);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductPackaging", new { productId = packagingType.ProductId });
        }

        #endregion

    }
}
