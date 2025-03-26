//using System.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using SmartLoad.Models;
//using SmartLoad.Services;
//using SmartLoad.Data;

//namespace SmartLoad.Controllers
//{
//    public class LoadingController : Controller
//    {
//        private readonly LoadingService _loadingService;
//        private readonly ApplicationDbContext _context;

//        public LoadingController(LoadingService loadingService, ApplicationDbContext context)
//        {
//            _context = context;
//            _loadingService = loadingService;
//        }
//        #region Методы управления схемами загрузки и продукции на этих схемах

//        #region LoadingSchemes

//        public IActionResult LoadingSchemes()
//        {
//            var loadingSchemes = _context.LoadingSchemes
//                .Include(ls => ls.VehicleType)
//                .Include(ls => ls.LoadingProducts)
//                .ThenInclude(lp => lp.Product)
//                .ThenInclude(p => p.PackagingTypes)
//                .ToList();
//            return View(loadingSchemes);
//        }

//        public IActionResult AddLoadingScheme()
//        {
//            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
//            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name");
//            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name");
//            return View();
//        }

//        [HttpPost]
//        public IActionResult AddLoadingScheme(LoadingScheme loadingScheme)
//        {
//            if (!ModelState.IsValid)
//            {
//                //try
//                //{
//                // Преобразуем дату загрузки в UTC
//                loadingScheme.LoadingDate = loadingScheme.LoadingDate.ToUniversalTime();

//                // Добавляем схему погрузки в контекст
//                _context.LoadingSchemes.Add(loadingScheme);
//                _context.SaveChanges();


//                // Перенаправляем пользователя к списку продуктов в новой схеме погрузки
//                return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingScheme.Id });
//                // }
//                // catch (Exception ex)
//                // {
//                //ModelState.AddModelError(string.Empty, ex.Message);
//                // }
//            }

//            // Если модель невалидна, повторно заполняем ViewData и возвращаем представление с ошибками
//            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
//            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name", loadingScheme.VehicleId);
//            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", loadingScheme.RouteId);
//            return View(loadingScheme);
//        }
        

//        public IActionResult LoadingSchemeDetails(int id)
//        {
//            var loadingScheme = _context.LoadingSchemes
//                .Include(ls => ls.VehicleType)
//                .Include(ls => ls.LoadingProducts)
//                .ThenInclude(lp => lp.Product)
//                .ThenInclude(p => p.PackagingTypes)
//                .FirstOrDefault(ls => ls.Id == id);

//            if (loadingScheme == null)
//            {
//                return NotFound();
//            }

//            ViewBag.LoadingSchemeId = id;
//            ViewBag.LoadingScheme = loadingScheme;

//            return View(loadingScheme.LoadingProducts);
//        }

//        [HttpGet]
//        public JsonResult GetLoadingSchemeDetails(int id)
//        {
//            try
//            {
//                var loadingScheme = _context.LoadingSchemes
//                    .Include(ls => ls.VehicleType)
//                    .Include(ls => ls.LoadingProducts)
//                    .ThenInclude(lp => lp.Product)
//                    .ThenInclude(p => p.PackagingTypes)
//                    .FirstOrDefault(ls => ls.Id == id);

//                if (loadingScheme != null)
//                {
//                    var result = new
//                    {
//                        id = loadingScheme.Id,
//                        name = loadingScheme.VehicleType.Name,
//                        vehicleType = new
//                        {
//                            TrailerLength = loadingScheme.VehicleType.Length,
//                            TrailerWidth = loadingScheme.VehicleType.Width
//                        },
//                        loadingProducts = loadingScheme.LoadingProducts.Select(lp => new LoadingProductDto
//                        {
//                            Id = lp.Id,
//                            ProductId = lp.ProductId,
//                            Quantity = lp.Quantity,
//                            PositionX = lp.PositionX,
//                            PositionY = lp.PositionY,
//                            PositionZ = lp.PositionZ,
//                            Product = new ProductDto
//                            {
//                                Id = lp.Product.Id,
//                                Name = lp.Product.Name
//                            },
//                            PackagingType = lp.Product.PackagingTypes.FirstOrDefault() != null ? new PackagingTypeDto
//                            {
//                                Id = lp.Product.PackagingTypes.FirstOrDefault().Id,
//                                Name = lp.Product.PackagingTypes.FirstOrDefault().Name,
//                                Length = lp.Product.PackagingTypes.FirstOrDefault().Length,
//                                Width = lp.Product.PackagingTypes.FirstOrDefault().Width,
//                                Height = lp.Product.PackagingTypes.FirstOrDefault().Height,
//                                Weight = lp.Product.PackagingTypes.FirstOrDefault().Weight
//                            } : null
//                        }).ToList()
//                    };

//                    // Логирование результатов
//                    Debug.WriteLine($"LoadingSchemeId: {result.id}");
//                    Debug.WriteLine($"VehicleType: {result.vehicleType.TrailerLength} x {result.vehicleType.TrailerWidth}");
//                    result.loadingProducts.ForEach(lp =>
//                    {
//                        Debug.WriteLine($"LoadingProductId: {lp.Id}, Product: {lp.Product.Name}, PackagingType: {lp.PackagingType?.Name ?? "Not found"}");
//                    });

//                    // Дополнительная проверка на сервере
//                    foreach (var lp in result.loadingProducts)
//                    {
//                        if (lp.PackagingType == null)
//                        {
//                            Debug.WriteLine($"Продукт с ID {lp.Id} не имеет связанного типа упаковки");
//                        }
//                    }

//                    return Json(result);
//                }

//                return Json(new { success = false, message = "Схема погрузки не найдена" });
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"Ошибка при получении данных о схеме погрузки: {ex.Message}");
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        public IActionResult EditLoadingScheme(int id)
//        {
//            var loadingScheme = _context.LoadingSchemes
//                .Include(ls => ls.VehicleType)
//                .Include(ls => ls.LoadingProducts)
//                .ThenInclude(lp => lp.Product)
//                .FirstOrDefault(ls => ls.Id == id);
//            if (loadingScheme == null)
//            {
//                return NotFound();
//            }

//            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
//            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name", loadingScheme.VehicleId);
//            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", loadingScheme.RouteId);
//            return View(loadingScheme);
//        }
//        [HttpPost]
//        public IActionResult EditLoadingScheme(LoadingScheme loadingScheme)
//        {
//            if (!ModelState.IsValid)
//            {
//                _context.LoadingSchemes.Update(loadingScheme);
//                _context.SaveChanges();
//                return RedirectToAction("LoadingSchemes");
//            }

//            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
//            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name", loadingScheme.VehicleId);
//            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", loadingScheme.RouteId);
//            return View(loadingScheme);
//        }

//        public IActionResult DeleteLoadingScheme(int id)
//        {
//            var loadingScheme = _context.LoadingSchemes
//                .Include(ls => ls.LoadingProducts)
//                .FirstOrDefault(ls => ls.Id == id);

//            if (loadingScheme == null)
//            {
//                return NotFound();
//            }
//            return View(loadingScheme);
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteLoadingSchemeConfirmed(int id)
//        {
//            var loadingScheme = await _context.LoadingSchemes
//                .Include(ls => ls.LoadingProducts)
//                .FirstOrDefaultAsync(ls => ls.Id == id);
//            if (loadingScheme == null)
//            {
//                return NotFound();
//            }
//            _context.LoadingSchemes.Remove(loadingScheme);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("LoadingSchemes");
//        }

//        #endregion

//        #region LoadingProducts

//        public IActionResult LoadingProducts(int loadingSchemeId)
//        {
//            var loadingProducts = _context.LoadingProducts
//                .Include(lp => lp.Product)
//                .ThenInclude(p => p.PackagingTypes)
//                .Include(lp => lp.LoadingScheme)
//                .Where(lp => lp.LoadingSchemeId == loadingSchemeId)
//            .ToList();

//            ViewBag.LoadingSchemeId = loadingSchemeId;
//            ViewBag.LoadingScheme = _context.LoadingSchemes
//                .Include(ls => ls.VehicleType)
//                .FirstOrDefault(ls => ls.Id == loadingSchemeId);

//            return View(loadingProducts);
//        }

//        public IActionResult AddLoadingProduct(int loadingSchemeId)
//        {
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
//            ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name");
//            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name");
//            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name");
//            ViewBag.LoadingSchemeId = loadingSchemeId;
//            return View();
//        }

//        [HttpPost]
//        public IActionResult AddLoadingProduct(LoadingProduct loadingProduct)
//        {
//            if (!ModelState.IsValid)
//            {
//                try
//                {
//                    // Проверяем существование связанного продукта
//                    var product = _context.Products
//                        .Include(p => p.PackagingTypes)
//                        .FirstOrDefault(p => p.Id == loadingProduct.ProductId);
//                    if (product == null)
//                    {
//                        ModelState.AddModelError("ProductId", "Выбранный продукт не существует");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Проверяем существование связанного типа упаковки
//                    var packagingType = product.PackagingTypes
//                        .FirstOrDefault(pt => pt.Id == loadingProduct.PackagingTypeId);
//                    if (packagingType == null)
//                    {
//                        ModelState.AddModelError("PackagingTypeId", "Выбранный тип упаковки не существует для данного продукта");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Проверяем существование связанного заказа
//                    var order = _context.Orders.Find(loadingProduct.OrderId);
//                    if (order == null)
//                    {
//                        ModelState.AddModelError("OrderId", "Выбранный заказ не существует");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Проверяем существование связанной точки маршрута
//                    var routePoint = _context.RoutePoints.Find(loadingProduct.RoutePointId);
//                    if (routePoint == null)
//                    {
//                        ModelState.AddModelError("RoutePointId", "Выбранная точка маршрута не существует");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Добавляем новый продукт в схему погрузки
//                    _context.LoadingProducts.Add(loadingProduct);
//                    _context.SaveChanges();
//                    return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError(string.Empty, ex.Message);
//                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                    ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                    return View(loadingProduct);
//                }
//            }

//            // Если модель невалидна, повторно заполняем ViewData и возвращаем представление с ошибками
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//            ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//            ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//            return View(loadingProduct);
//        }
//        public IActionResult EditLoadingProduct(int id)
//        {
//            var loadingProduct = _context.LoadingProducts
//                .Include(lp => lp.Product)
//                .ThenInclude(p => p.PackagingTypes)
//                .Include(lp => lp.LoadingScheme)
//                .Include(lp => lp.Order)
//                .Include(lp => lp.RoutePoint)
//                .FirstOrDefault(lp => lp.Id == id);

//            if (loadingProduct == null)
//            {
//                return NotFound();
//            }

//            ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//            ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//            return View(loadingProduct);
//        }

//        [HttpPost]
//        public IActionResult EditLoadingProduct(LoadingProduct loadingProduct)
//        {
//            if (!ModelState.IsValid)
//            {
//                try
//                {
//                    // Проверяем существование связанного продукта
//                    var product = _context.Products
//                        .Include(p => p.PackagingTypes)
//                        .FirstOrDefault(p => p.Id == loadingProduct.ProductId);
//                    if (product == null)
//                    {
//                        ModelState.AddModelError("ProductId", "Выбранный продукт не существует");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Проверяем существование связанного типа упаковки
//                    var packagingType = product.PackagingTypes
//                        .FirstOrDefault(pt => pt.Id == loadingProduct.PackagingTypeId);
//                    if (packagingType == null)
//                    {
//                        ModelState.AddModelError("PackagingTypeId", "Выбранный тип упаковки не существует для данного продукта");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Проверяем существование связанного заказа
//                    var order = _context.Orders.Find(loadingProduct.OrderId);
//                    if (order == null)
//                    {
//                        ModelState.AddModelError("OrderId", "Выбранный заказ не существует");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Проверяем существование связанной точки маршрута
//                    var routePoint = _context.RoutePoints.Find(loadingProduct.RoutePointId);
//                    if (routePoint == null)
//                    {
//                        ModelState.AddModelError("RoutePointId", "Выбранная точка маршрута не существует");
//                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                        ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                        return View(loadingProduct);
//                    }

//                    // Обновляем продукт в схеме погрузки
//                    _context.LoadingProducts.Update(loadingProduct);
//                    _context.SaveChanges();
//                    return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError(string.Empty, ex.Message);
//                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//                    ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//                    return View(loadingProduct);
//                }
//            }

//            // Если модель невалидна, повторно заполняем ViewData и возвращаем представление с ошибками
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//            ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
//            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
//            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
//            ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//            return View(loadingProduct);
//        }
//        public IActionResult DeleteLoadingProduct(int id)
//        {
//            var loadingProduct = _context.LoadingProducts
//                .Include(lp => lp.Product)
//                .ThenInclude(p => p.PackagingTypes)
//                .Include(lp => lp.LoadingScheme)
//                .Include(lp => lp.Order)
//                .Include(lp => lp.RoutePoint)
//                .FirstOrDefault(lp => lp.Id == id);

//            if (loadingProduct == null)
//            {
//                return NotFound();
//            }

//            ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//            return View(loadingProduct);
//        }

//        [HttpPost, ActionName("DeleteLoadingProduct")]
//        public async Task<IActionResult> DeleteLoadingProductConfirmed(int id)
//        {
//            var loadingProduct = await _context.LoadingProducts
//                .FirstOrDefaultAsync(lp => lp.Id == id);

//            if (loadingProduct == null)
//            {
//                return NotFound();
//            }

//            _context.LoadingProducts.Remove(loadingProduct);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
//        }
//        #endregion
//        [HttpGet]
//        public JsonResult GetProductDetails(int id)
//        {
//            try
//            {
//                var product = _context.Products
//                    .Include(p => p.PackagingTypes)
//                    .FirstOrDefault(p => p.Id == id);

//                if (product != null)
//                {
//                    return Json(new
//                    {
//                        id = product.Id,
//                        name = product.Name,
//                        packagingTypes = product.PackagingTypes.Select(pt => new
//                        {
//                            id = pt.Id,
//                            name = pt.Name
//                        }).ToList()
//                    });
//                }

//                return Json(new { success = false, message = "Продукт не найден" });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        [HttpGet]
//        public JsonResult GetProductPackagingDetails(int id)
//        {
//            var packagingType = _context.PackagingTypes
//                .FirstOrDefault(pt => pt.Id == id);

//            if (packagingType != null)
//            {
//                return Json(new
//                {
//                    id = packagingType.Id,
//                    name = packagingType.Name,
//                    length = packagingType.Length,
//                    width = packagingType.Width,
//                    height = packagingType.Height,
//                    weight = packagingType.Weight
//                });
//            }

//            return Json(null);
//        }

//        #endregion

//        //[HttpPost]
//        //public IActionResult CreateLoadingScheme([FromBody] List<LoadingProduct> products)
//        //{
//        //    try
//        //    {
//        //        // Пример размеров и грузоподъемности прицепа
//        //        var placedProducts = _loadingService.PlaceProducts(products, 16, 2.5f, 3.0f, 20000);

//        //        return Ok(placedProducts);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return BadRequest(ex.Message);
//        //    }
//        //}
//    }
//}
