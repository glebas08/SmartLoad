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

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly LoadingService _loadingService;

    public HomeController(ApplicationDbContext context, LoadingService loadingService)
    {
        _context = context;
        _loadingService = loadingService;
    }

    public IActionResult Index()
    {
        return View();
    }

    //Входные данные
    public IActionResult VhodData()
    {
        return View();
    }

}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using SmartLoad.Data;
//using SmartLoad.Models;
//using SmartLoad.Services;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//public class HomeController : Controller
//{
//    private readonly ApplicationDbContext _context;
//    private readonly LoadingService _loadingService;

//    public HomeController(ApplicationDbContext context, LoadingService loadingService)
//    {
//        _context = context;
//        _loadingService = loadingService;
//    }

//    public IActionResult Index()
//    {
//        return View();
//    }

//    public IActionResult TypesOfVehicles()
//    {
//        var vehicleTypes = _context.VehicleTypes.ToList();
//        return View(vehicleTypes);
//    }

//    #region ТС

//    public IActionResult AddVehicleType()
//    {
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AddVehicleType(VehicleType vehicleType)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.VehicleTypes.Add(vehicleType);
//            _context.SaveChanges();
//            return RedirectToAction("TypesOfVehicles");
//        }
//        return View(vehicleType);
//    }

//    public IActionResult DeleteVehicleType(int id)
//    {
//        var vehicleType = _context.VehicleTypes.Find(id);
//        if (vehicleType == null)
//        {
//            return NotFound();
//        }
//        return View(vehicleType);
//    }

//    [HttpPost, ActionName("DeleteConfirmed")]
//    public async Task<IActionResult> DeleteConfirmed(int id)
//    {
//        var vehicle = await _context.VehicleTypes.FindAsync(id);
//        if (vehicle == null)
//        {
//            return NotFound();
//        }

//        _context.VehicleTypes.Remove(vehicle);
//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(TypesOfVehicles));
//    }

//    public IActionResult EditVehicleType(int id)
//    {
//        var vehicleType = _context.VehicleTypes.Find(id);
//        if (vehicleType == null)
//        {
//            return NotFound();
//        }
//        return View(vehicleType);
//    }

//    [HttpPost]
//    public IActionResult EditVehicleType(VehicleType vehicleType)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.VehicleTypes.Update(vehicleType);
//            _context.SaveChanges();
//            return RedirectToAction("TypesOfVehicles");
//        }
//        return View(vehicleType);
//    }

//    #endregion

//    #region Продукт

//    public IActionResult Products()
//    {
//        var products = GetProducts();
//        return View(products);
//    }

//    private List<Product> GetProducts()
//    {
//        return _context.Products
//            .Include(p => p.PackagingTypes)
//            .ToList();
//    }

//    public IActionResult AddProduct()
//    {
//        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name");
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AddProduct(Product product)
//    {
//        try
//        {
//            _context.Products.Add(product);
//            _context.SaveChanges();
//            return RedirectToAction("Products");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при сохранении продукта: {ex.Message}");
//            return View(product);
//        }
//    }

//    public IActionResult EditProduct(int id)
//    {
//        var product = _context.Products
//            .Include(p => p.PackagingTypes)
//            .FirstOrDefault(p => p.Id == id);

//        if (product == null)
//        {
//            return NotFound();
//        }
//        return View(product);
//    }

//    [HttpPost]
//    public IActionResult EditProduct(Product product)
//    {
//        if (!ModelState.IsValid)
//        {
//            _context.Products.Update(product);
//            _context.SaveChanges();
//            return RedirectToAction("Products");
//        }
//        return View(product);
//    }

//    public IActionResult DeleteProduct(int id)
//    {
//        var product = _context.Products
//            .Include(p => p.PackagingTypes)
//            .FirstOrDefault(p => p.Id == id);

//        if (product == null)
//        {
//            return NotFound();
//        }
//        return View(product);
//    }

//    [HttpPost, ActionName("DeleteProduct")]
//    public async Task<IActionResult> DeleteProductConfirmed(int id)
//    {
//        var product = await _context.Products.FindAsync(id);
//        if (product == null)
//        {
//            return NotFound();
//        }
//        _context.Products.Remove(product);
//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Products));
//    }

//    #endregion

//    #region Тип упаковки

//    public IActionResult PackagingTypes()
//    {
//        var packagingTypes = _context.PackagingTypes.ToList();
//        return View(packagingTypes);
//    }

//    public IActionResult AddPackagingType()
//    {
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AddPackagingType(PackagingType packagingType)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.PackagingTypes.Add(packagingType);
//            _context.SaveChanges();
//            return RedirectToAction("PackagingTypes");
//        }
//        return View(packagingType);
//    }

//    public IActionResult EditPackagingType(int id)
//    {
//        var packagingType = _context.PackagingTypes
//            .Include(pt => pt.Product)
//            .FirstOrDefault(pt => pt.Id == id);

//        if (packagingType == null)
//        {
//            return NotFound();
//        }

//        ViewBag.ProductId = packagingType.ProductId;
//        ViewBag.ProductName = packagingType.Product.Name;

//        return View(packagingType);
//    }

//    [HttpPost]
//    public IActionResult EditPackagingType(PackagingType packagingType)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.PackagingTypes.Update(packagingType);
//            _context.SaveChanges();
//            return RedirectToAction("ProductPackaging", new { productId = packagingType.ProductId });
//        }

//        ViewBag.ProductId = packagingType.ProductId;
//        ViewBag.ProductName = _context.Products.Find(packagingType.ProductId)?.Name;

//        return View(packagingType);
//    }

//    public IActionResult DeletePackagingType(int id)
//    {
//        var packagingType = _context.PackagingTypes
//            .Include(pt => pt.Product)
//            .FirstOrDefault(pt => pt.Id == id);

//        if (packagingType == null)
//        {
//            return NotFound();
//        }

//        ViewBag.ProductId = packagingType.ProductId;
//        ViewBag.ProductName = packagingType.Product.Name;

//        return View(packagingType);
//    }

//    [HttpPost, ActionName("DeletePackagingType")]
//    public async Task<IActionResult> DeletePackagingTypeConfirmed(int id)
//    {
//        var packagingType = await _context.PackagingTypes.FindAsync(id);
//        if (packagingType == null)
//        {
//            return NotFound();
//        }

//        _context.PackagingTypes.Remove(packagingType);
//        await _context.SaveChangesAsync();
//        return RedirectToAction("ProductPackaging", new { productId = packagingType.ProductId });
//    }

//    #endregion

//    #region Маршруты

//    public IActionResult Routes()
//    {
//        var routes = _context.Routes
//            .Include(r => r.RoutePoints)
//            .ThenInclude(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(r => r.LoadingSchemes)
//            .ThenInclude(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .ToList();
//        return View(routes);
//    }

//    public IActionResult AddRoute()
//    {
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AddRoute(SmartLoad.Models.Rout route)
//    {
//        if (!ModelState.IsValid)
//        {
//            _context.Routes.Add(route);
//            _context.SaveChanges();
//            return RedirectToAction("Routes");
//        }
//        return View(route);
//    }

//    public IActionResult EditRoute(int id)
//    {
//        var route = _context.Routes
//            .Include(r => r.RoutePoints)
//            .ThenInclude(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(r => r.LoadingSchemes)
//            .ThenInclude(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefault(r => r.Id == id);

//        if (route == null)
//        {
//            return NotFound();
//        }

//        return View(route);
//    }

//    [HttpPost]
//    public IActionResult EditRoute(SmartLoad.Models.Rout route)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.Routes.Update(route);
//            _context.SaveChanges();
//            return RedirectToAction("Routes");
//        }

//        return View(route);
//    }

//    public IActionResult DeleteRoute(int id)
//    {
//        var route = _context.Routes
//            .Include(r => r.RoutePoints)
//            .ThenInclude(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(r => r.LoadingSchemes)
//            .ThenInclude(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefault(r => r.Id == id);

//        if (route == null)
//        {
//            return NotFound();
//        }

//        return View(route);
//    }

//    [HttpPost, ActionName("DeleteRoute")]
//    public async Task<IActionResult> DeleteRouteConfirmed(int id)
//    {
//        var route = await _context.Routes
//            .Include(r => r.RoutePoints)
//            .ThenInclude(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(r => r.LoadingSchemes)
//            .ThenInclude(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefaultAsync(r => r.Id == id);

//        if (route == null)
//        {
//            return NotFound();
//        }

//        _context.Routes.Remove(route);
//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Routes));
//    }

//    public IActionResult RoutePoints(int routeId)
//    {
//        var routePoints = _context.RoutePoints
//            .Include(rp => rp.Rout)
//            .Include(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(rp => rp.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .Where(rp => rp.RouteId == routeId)
//            .ToList();
//        ViewBag.RouteId = routeId;
//        return View(routePoints);
//    }

//    public IActionResult AddRoutePoint(int routeId)
//    {
//        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name");
//        ViewBag.RouteId = routeId;
//        return View(new RoutePoint { RouteId = routeId });
//    }

//    [HttpPost]
//    public IActionResult AddRoutePoint(RoutePoint routePoint, int[] selectedOrderIds)
//    {
//        if (ModelState.IsValid)
//        {
//            // Добавление новой точки маршрута в контекст
//            _context.RoutePoints.Add(routePoint);
//            _context.SaveChanges();

//            // Инициализация коллекции OrderRoutePoints
//            routePoint.OrderRoutePoints = new List<OrderRoutePoint>();

//            // Проверка наличия выбранных заказов
//            if (selectedOrderIds != null)
//            {
//                foreach (var orderId in selectedOrderIds)
//                {
//                    // Поиск заказа по ID
//                    var order = _context.Orders.Find(orderId);
//                    if (order != null)
//                    {
//                        // Добавление связи между точкой маршрута и заказом
//                        routePoint.OrderRoutePoints.Add(new OrderRoutePoint
//                        {
//                            OrderId = orderId,
//                            RoutePointId = routePoint.Id
//                        });
//                    }
//                }
//            }

//            // Сохранение связей OrderRoutePoints
//            _context.SaveChanges();

//            // Перенаправление на страницу со списком точек маршрута
//            return RedirectToAction("RoutePoints", new { routeId = routePoint.RouteId });
//        }

//        // Если модель недействительна, повторное заполнение SelectList
//        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name", selectedOrderIds ?? Array.Empty<int>());
//        ViewBag.RouteId = routePoint.RouteId;

//        // Возврат представления с ошибками валидации
//        return View(routePoint);
//    }

//    public IActionResult EditRoutePoint(int id)
//    {
//        var routePoint = _context.RoutePoints
//            .Include(rp => rp.Rout)
//            .Include(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(rp => rp.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefault(rp => rp.Id == id);

//        if (routePoint == null)
//        {
//            return NotFound();
//        }

//        var selectedOrderIds = routePoint.OrderRoutePoints.Select(orp => orp.OrderId).ToArray();
//        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name", selectedOrderIds);
//        ViewBag.RouteId = routePoint.RouteId;
//        return View(routePoint);
//    }

//    [HttpPost]
//    public IActionResult EditRoutePoint(RoutePoint routePoint, int[] selectedOrderIds)
//    {
//        if (ModelState.IsValid)
//        {
//            // Очистка существующих связей
//            routePoint.OrderRoutePoints = new List<OrderRoutePoint>();

//            // Добавление новых связей
//            if (selectedOrderIds != null)
//            {
//                foreach (var orderId in selectedOrderIds)
//                {
//                    var orderRoutePoint = new OrderRoutePoint
//                    {
//                        OrderId = orderId,
//                        RoutePointId = routePoint.Id
//                    };
//                    _context.OrderRoutePoints.Add(orderRoutePoint);
//                }
//                _context.SaveChanges();
//            }

//            _context.RoutePoints.Update(routePoint);
//            _context.SaveChanges();
//            return RedirectToAction("RoutePoints", new { routeId = routePoint.RouteId });
//        }

//        var existingSelectedOrderIds = routePoint.OrderRoutePoints.Select(orp => orp.OrderId).ToArray();
//        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name", existingSelectedOrderIds);
//        ViewBag.RouteId = routePoint.RouteId;
//        return View(routePoint);
//    }

//    public IActionResult DeleteRoutePoint(int id)
//    {
//        var routePoint = _context.RoutePoints
//            .Include(rp => rp.Rout)
//            .Include(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(rp => rp.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefault(rp => rp.Id == id);

//        if (routePoint == null)
//        {
//            return NotFound();
//        }

//        ViewBag.RouteId = routePoint.RouteId;
//        return View(routePoint);
//    }

//    [HttpPost, ActionName("DeleteRoutePoint")]
//    public async Task<IActionResult> DeleteRoutePointConfirmed(int id)
//    {
//        var routePoint = await _context.RoutePoints
//            .Include(rp => rp.Rout)
//            .Include(rp => rp.OrderRoutePoints)
//            .ThenInclude(orp => orp.Order)
//            .Include(rp => rp.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefaultAsync(rp => rp.Id == id);

//        if (routePoint == null)
//        {
//            return NotFound();
//        }

//        _context.RoutePoints.Remove(routePoint);
//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(RoutePoints), new { routeId = routePoint.RouteId });
//    }

//    #endregion

//    #region Методы управления схемами загрузки и продукции на этих схемах

//    public IActionResult LoadingSchemes()
//    {
//        var loadingSchemes = _context.LoadingSchemes
//            .Include(ls => ls.VehicleType)
//            .Include(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .ToList();
//        return View(loadingSchemes);
//    }

//    public IActionResult AddLoadingScheme()
//    {
//        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AddLoadingScheme(LoadingScheme loadingScheme)
//    {
//        if (ModelState.IsValid)
//        {
//            try
//            {
//                var createdScheme = _loadingService.CreateLoadingScheme(loadingScheme.VehicleId, loadingScheme.RouteId);
//                return RedirectToAction("LoadingProducts", new { loadingSchemeId = createdScheme.Id });
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError(string.Empty, ex.Message);
//            }
//        }
//        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
//        return View(loadingScheme);
//    }

//    public IActionResult LoadingProducts(int loadingSchemeId)
//    {
//        var loadingProducts = _context.LoadingProducts
//            .Include(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .Include(lp => lp.LoadingScheme)
//            .Where(lp => lp.LoadingSchemeId == loadingSchemeId)
//            .ToList();

//        ViewBag.LoadingSchemeId = loadingSchemeId;
//        return View(loadingProducts);
//    }

//    public IActionResult AddLoadingProduct(int loadingSchemeId)
//    {
//        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
//        ViewBag.LoadingSchemeId = loadingSchemeId;
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AddLoadingProduct(LoadingProduct loadingProduct)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.LoadingProducts.Add(loadingProduct);
//            _context.SaveChanges();
//            return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
//        }
//        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
//        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
//        return View(loadingProduct);
//    }

//    [HttpGet]
//    public JsonResult GetProductDetails(int id)
//    {
//        var product = _context.Products
//            .FirstOrDefault(p => p.Id == id);

//        if (product != null)
//        {
//            return Json(new
//            {
//                id = product.Id,
//                name = product.Name,
//            });
//        }

//        return Json(null);
//    }

//    public IActionResult LoadingSchemeDetails(int id)
//    {
//        var loadingScheme = _context.LoadingSchemes
//            .Include(ls => ls.VehicleType)
//            .Include(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefault(ls => ls.Id == id);

//        if (loadingScheme == null)
//        {
//            return NotFound();
//        }

//        ViewBag.LoadingSchemeId = id;
//        ViewBag.LoadingScheme = loadingScheme;

//        return View(loadingScheme.LoadingProducts);
//    }

//    [HttpGet]
//    public JsonResult GetLoadingSchemeDetails(int id)
//    {
//        var loadingScheme = _context.LoadingSchemes
//            .Include(ls => ls.VehicleType)
//            .Include(ls => ls.LoadingProducts)
//            .ThenInclude(lp => lp.Product)
//            .ThenInclude(p => p.PackagingTypes)
//            .FirstOrDefault(ls => ls.Id == id);

//        if (loadingScheme != null)
//        {
//            return Json(new
//            {
//                id = loadingScheme.Id,
//                name = loadingScheme.VehicleType.Name,
//                vehicleType = new
//                {
//                    TrailerLength = loadingScheme.VehicleType.Length,
//                    TrailerWidth = loadingScheme.VehicleType.Width
//                },
//                loadingProducts = loadingScheme.LoadingProducts.Select(lp => new
//                {
//                    lp.Id,
//                    lp.ProductId,
//                    lp.Quantity,
//                    lp.PositionX,
//                    lp.PositionY,
//                    lp.PositionZ,
//                    product = lp.Product,
//                    packagingType = lp.Product.PackagingTypes
//                }).ToList()
//            });
//        }

//        return Json(null);
//    }

//    #endregion
//}
