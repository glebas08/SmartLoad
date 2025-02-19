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

    public IActionResult TypesOfVehicles()
    {
        var vehicleTypes = _context.VehicleTypes.ToList();
        return View(vehicleTypes);
    }

    #region ТС

    #region ТипТС
    public IActionResult AddVehicleType()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddVehicleType(VehicleType vehicleType)
    {
        if (!ModelState.IsValid)
        {
            _context.VehicleTypes.Add(vehicleType);
            _context.SaveChanges();
            return RedirectToAction("TypesOfVehicles");
        }
        return View(vehicleType);
    }

    public IActionResult DeleteVehicleType(int id)
    {
        var vehicleType = _context.VehicleTypes.Find(id);
        if (vehicleType == null)
        {
            return NotFound();
        }
        return View(vehicleType);
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var vehicle = await _context.VehicleTypes.FindAsync(id);
        if (vehicle == null)
        {
            return NotFound();
        }

        _context.VehicleTypes.Remove(vehicle);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(TypesOfVehicles));
    }

    public IActionResult EditVehicleType(int id)
    {
        var vehicleType = _context.VehicleTypes.Find(id);
        if (vehicleType == null)
        {
            return NotFound();
        }
        return View(vehicleType);
    }

    [HttpPost]
    public IActionResult EditVehicleType(VehicleType vehicleType)
    {
        if (!ModelState.IsValid)
        {
            _context.VehicleTypes.Update(vehicleType);
            _context.SaveChanges();
            return RedirectToAction("TypesOfVehicles");
        }
        return View(vehicleType);
    }

    #endregion

    #region Транстпорные средства

    public IActionResult Vehicles()
    {
        var vehicles = _context.Vehicles.Include(v => v.VehicleType).ToList();
        return View(vehicles);
    }

    public IActionResult AddVehicle()
    {
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult AddVehicle(Vehicle vehicle)
    {
        if (!ModelState.IsValid)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return RedirectToAction("Vehicles");
        }
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
        return View(vehicle);
    }

    public IActionResult EditVehicle(int id)
    {
        var vehicle = _context.Vehicles.Find(id);
        if (vehicle == null)
        {
            return NotFound();
        }
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
        return View(vehicle);
    }

    [HttpPost]
    public IActionResult EditVehicle(Vehicle vehicle)
    {
        if (!ModelState.IsValid)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
            return RedirectToAction("Vehicles");
        }
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
        return View(vehicle);
    }

    public IActionResult DeleteVehicle(int id)
    {
        var vehicle = _context.Vehicles.Find(id);
        if (vehicle == null)
        {
            return NotFound();
        }
        return View(vehicle);
    }

    [HttpPost, ActionName("DeleteVehicle")]
    public async Task<IActionResult> DeleteVehicleConfirmed(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null)
        {
            return NotFound();
        }
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Vehicles));
    }

    #endregion

    #endregion

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

    #region Маршруты
    #region по которому будут доставлятся заказы
    public IActionResult Routes()
    {
        var routes = _context.Routes
            .Include(r => r.RoutePoints)
            .ThenInclude(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(r => r.LoadingSchemes)
            .ThenInclude(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .ToList();

        return View(routes);
    }

    public IActionResult AddRoute()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddRoute(SmartLoad.Models.Rout route)
    {
        if (!ModelState.IsValid)
        {
            // Преобразование дат в UTC
            route.DepartureDate = DateTime.SpecifyKind(route.DepartureDate, DateTimeKind.Utc);
            route.ArrivalDate = DateTime.SpecifyKind(route.ArrivalDate, DateTimeKind.Utc);
            route.RouteDate = DateTime.SpecifyKind(route.RouteDate, DateTimeKind.Utc);

            _context.Routes.Add(route);
            _context.SaveChanges();
            return RedirectToAction("Routes");
        }
        return View(route);
    }

    public IActionResult EditRoute(int id)
    {
        var route = _context.Routes
            .Include(r => r.RoutePoints)
            .ThenInclude(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(r => r.LoadingSchemes)
            .ThenInclude(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefault(r => r.Id == id);

        if (route == null)
        {
            return NotFound();
        }

        return View(route);
    }

    [HttpPost]
    public IActionResult EditRoute(SmartLoad.Models.Rout route)
    {
        if (!ModelState.IsValid)
        {
            _context.Routes.Update(route);
            _context.SaveChanges();
            return RedirectToAction("Routes");
        }

        return View(route);
    }

    public IActionResult DeleteRoute(int id)
    {
        var route = _context.Routes
            .Include(r => r.RoutePoints)
            .ThenInclude(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(r => r.LoadingSchemes)
            .ThenInclude(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefault(r => r.Id == id);

        if (route == null)
        {
            return NotFound();
        }

        return View(route);
    }

    [HttpPost, ActionName("DeleteRoute")]
    public async Task<IActionResult> DeleteRouteConfirmed(int id)
    {
        var route = await _context.Routes
            .Include(r => r.RoutePoints)
            .ThenInclude(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(r => r.LoadingSchemes)
            .ThenInclude(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (route == null)
        {
            return NotFound();
        }

        _context.Routes.Remove(route);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Routes));
    }

    #endregion

    #region Точки выгрузки
    public IActionResult RoutePoints(int routeId)
    {
        var routePoints = _context.RoutePoints
            .Include(rp => rp.Rout)
            .Include(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(rp => rp.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .Where(rp => rp.RouteId == routeId)
            .ToList();

        ViewBag.RouteId = routeId;
        return View(routePoints);
    }

    public IActionResult AddRoutePoint(int routeId)
    {
        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name");
        ViewBag.RouteId = routeId;
        return View(new RoutePoint { RouteId = routeId });
    }

    [HttpPost]
    public IActionResult AddRoutePoint(RoutePoint routePoint, int[] selectedOrderIds)
    {
        if (!ModelState.IsValid)
        {
            routePoint.UnloadingDate = DateTime.SpecifyKind(routePoint.UnloadingDate, DateTimeKind.Utc);
            // Добавление новой точки маршрута в контекст
            _context.RoutePoints.Add(routePoint);
            _context.SaveChanges();

            // Инициализация коллекции OrderRoutePoints
            routePoint.OrderRoutePoints = new List<OrderRoutePoint>();

            // Проверка наличия выбранных заказов
            if (selectedOrderIds != null)
            {
                foreach (var orderId in selectedOrderIds)
                {
                    // Поиск заказа по ID
                    var order = _context.Orders.Find(orderId);
                    if (order != null)
                    {
                        // Добавление связи между точкой маршрута и заказом
                        routePoint.OrderRoutePoints.Add(new OrderRoutePoint
                        {
                            OrderId = orderId,
                            RoutePointId = routePoint.Id
                        });
                    }
                }
            }

            // Сохранение связей OrderRoutePoints
            _context.SaveChanges();

            // Перенаправление на страницу со списком точек маршрута
            return RedirectToAction("RoutePoints", new { routeId = routePoint.RouteId });
        }

        // Если модель недействительна, повторное заполнение SelectList
        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name", selectedOrderIds ?? Array.Empty<int>());
        ViewBag.RouteId = routePoint.RouteId;

        // Возврат представления с ошибками валидации
        return View(routePoint);
    }
    public IActionResult EditRoutePoint(int id)
    {
        var routePoint = _context.RoutePoints
            .Include(rp => rp.Rout)
            .Include(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(rp => rp.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefault(rp => rp.Id == id);

        if (routePoint == null)
        {
            return NotFound();
        }

        var selectedOrderIds = routePoint.OrderRoutePoints.Select(orp => orp.OrderId).ToArray();
        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name", selectedOrderIds);
        ViewBag.RouteId = routePoint.RouteId;
        return View(routePoint);
    }

    [HttpPost]
    public IActionResult EditRoutePoint(RoutePoint routePoint, int[] selectedOrderIds)
    {
        if (!ModelState.IsValid)
        {
            routePoint.OrderRoutePoints = new List<OrderRoutePoint>();

            if (selectedOrderIds != null)
            {
                foreach (var orderId in selectedOrderIds)
                {
                    var order = _context.Orders.Find(orderId);
                    if (order != null)
                    {
                        routePoint.OrderRoutePoints.Add(new OrderRoutePoint { OrderId = orderId, RoutePointId = routePoint.Id });
                    }
                }
            }

            _context.RoutePoints.Update(routePoint);
            _context.SaveChanges();
            return RedirectToAction("RoutePoints", new { routeId = routePoint.RouteId });
        }

        var existingSelectedOrderIds = routePoint.OrderRoutePoints.Select(orp => orp.OrderId).ToArray();
        ViewData["OrderId"] = new MultiSelectList(_context.Orders, "Id", "Name", existingSelectedOrderIds);
        ViewBag.RouteId = routePoint.RouteId;
        return View(routePoint);
    }

    public IActionResult DeleteRoutePoint(int id)
    {
        var routePoint = _context.RoutePoints
            .Include(rp => rp.Rout)
            .Include(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(rp => rp.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefault(rp => rp.Id == id);

        if (routePoint == null)
        {
            return NotFound();
        }

        ViewBag.RouteId = routePoint.RouteId;
        return View(routePoint);
    }

    [HttpPost, ActionName("DeleteRoutePoint")]
    public async Task<IActionResult> DeleteRoutePointConfirmed(int id)
    {
        var routePoint = await _context.RoutePoints
            .Include(rp => rp.Rout)
            .Include(rp => rp.OrderRoutePoints)
            .ThenInclude(orp => orp.Order)
            .Include(rp => rp.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefaultAsync(rp => rp.Id == id);

        if (routePoint == null)
        {
            return NotFound();
        }

        _context.RoutePoints.Remove(routePoint);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(RoutePoints), new { routeId = routePoint.RouteId });
    }

    #endregion

    #endregion

    #region Заказы

    #region Заказ
    public IActionResult Orders()
    {
        var orders = _context.Orders.Include(o => o.Rout).ToList();
        return View(orders);
    }

    public IActionResult AddOrder()
    {
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult AddOrder(Order order)
    {
        if (!ModelState.IsValid)
        {
            // Установка значения по умолчанию для Notes, если оно не указано
            order.Notes = order.Notes ?? string.Empty;

            // Преобразование даты в UTC
            order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);

            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Orders");
        }
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", order.RouteId);
        return View(order);
    }

    public IActionResult EditOrder(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
        {
            return NotFound();
        }
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", order.RouteId);
        return View(order);
    }

    [HttpPost]
    public IActionResult EditOrder(Order order)
    {
        if (!ModelState.IsValid)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Orders");
        }
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", order.RouteId);
        return View(order);
    }

    public IActionResult DeleteOrder(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost, ActionName("DeleteOrder")]
    public async Task<IActionResult> DeleteOrderConfirmed(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Orders));
    }

    #endregion

    #region Продукты в заказе

    public IActionResult OrderProducts(int orderId)
    {
        var orderProducts = _context.OrderProducts
            .Include(op => op.Product)
            .Include(op => op.Order)
            .Where(op => op.OrderId == orderId)
            .ToList();

        ViewBag.OrderId = orderId;
        return View(orderProducts);
    }

    public IActionResult AddOrderProduct(int orderId)
    {
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
        ViewBag.OrderId = orderId;
        return View();
    }

    [HttpPost]
    public IActionResult AddOrderProduct(OrderProduct orderProduct)
    {
        if (!ModelState.IsValid)
        {
            _context.OrderProducts.Add(orderProduct);
            _context.SaveChanges();
            return RedirectToAction("OrderProducts", new { orderId = orderProduct.OrderId });
        }
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderProduct.ProductId);
        ViewBag.OrderId = orderProduct.OrderId;
        return View(orderProduct);
    }

    public IActionResult EditOrderProduct(int id)
    {
        var orderProduct = _context.OrderProducts
            .Include(op => op.Product)
            .Include(op => op.Order)
            .FirstOrDefault(op => op.Id == id);

        if (orderProduct == null)
        {
            return NotFound();
        }

        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderProduct.ProductId);
        ViewBag.OrderId = orderProduct.OrderId;
        return View(orderProduct);
    }

    [HttpPost]
    public IActionResult EditOrderProduct(OrderProduct orderProduct)
    {
        if (ModelState.IsValid)
        {
            _context.OrderProducts.Update(orderProduct);
            _context.SaveChanges();
            return RedirectToAction("OrderProducts", new { orderId = orderProduct.OrderId });
        }

        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderProduct.ProductId);
        ViewBag.OrderId = orderProduct.OrderId;
        return View(orderProduct);
    }

    public IActionResult DeleteOrderProduct(int id)
    {
        var orderProduct = _context.OrderProducts
            .Include(op => op.Product)
            .Include(op => op.Order)
            .FirstOrDefault(op => op.Id == id);

        if (orderProduct == null)
        {
            return NotFound();
        }

        ViewBag.OrderId = orderProduct.OrderId;
        return View(orderProduct);
    }

    [HttpPost, ActionName("DeleteOrderProduct")]
    public async Task<IActionResult> DeleteOrderProductConfirmed(int id)
    {
        var orderProduct = await _context.OrderProducts.FindAsync(id);
        if (orderProduct == null)
        {
            return NotFound();
        }

        _context.OrderProducts.Remove(orderProduct);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(OrderProducts), new { orderId = orderProduct.OrderId });
    }

    #endregion

    #region Связь между заказами и точками маршрута

    public IActionResult OrderRoutePoints(int orderId)
    {
        var orderRoutePoints = _context.OrderRoutePoints
            .Include(orp => orp.Order)
            .Include(orp => orp.RoutePoint)
            .Where(orp => orp.OrderId == orderId)
            .ToList();
        ViewBag.OrderId = orderId;
        return View(orderRoutePoints);
    }

    public IActionResult AddOrderRoutePoint(int orderId)
    {
        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name");
        ViewBag.OrderId = orderId;
        return View(new OrderRoutePoint { OrderId = orderId });
    }

    [HttpPost]
    public IActionResult AddOrderRoutePoint(OrderRoutePoint orderRoutePoint)
    {
        if (!ModelState.IsValid)
        {
            try
            {
                // Убедимся, что RoutePointId установлен правильно
                if (orderRoutePoint.RoutePointId == 0)
                {
                    ModelState.AddModelError(string.Empty, "Точка маршрута не выбрана.");
                    return View(orderRoutePoint);
                }

                // Проверка на существование записи OrderRoutePoint
                var existingOrderRoutePoint = _context.OrderRoutePoints
                    .FirstOrDefault(orp => orp.OrderId == orderRoutePoint.OrderId && orp.RoutePointId == orderRoutePoint.RoutePointId);

                if (existingOrderRoutePoint != null)
                {
                    ModelState.AddModelError(string.Empty, $"Связь между заказом с ID {orderRoutePoint.OrderId} и точкой маршрута с ID {orderRoutePoint.RoutePointId} уже существует.");
                    return View(orderRoutePoint);
                }

                // Добавляем новую связь
                _context.OrderRoutePoints.Add(orderRoutePoint);
                _context.SaveChanges();

                // Перенаправляем пользователя к списку связей между заказами и точками маршрута
                return RedirectToAction("OrderRoutePoints", new { orderId = orderRoutePoint.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(orderRoutePoint);
            }
        }

        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", orderRoutePoint.RoutePointId);
        ViewBag.OrderId = orderRoutePoint.OrderId;
        return View(orderRoutePoint);
    }

    public IActionResult EditOrderRoutePoint(int orderId, int routePointId)
    {
        var orderRoutePoint = _context.OrderRoutePoints
            .Include(orp => orp.Order)
            .Include(orp => orp.RoutePoint)
            .FirstOrDefault(orp => orp.OrderId == orderId && orp.RoutePointId == routePointId);

        if (orderRoutePoint == null)
        {
            return NotFound();
        }

        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", orderRoutePoint.RoutePointId);
        ViewBag.OrderId = orderRoutePoint.OrderId;
        return View(orderRoutePoint);
    }

    [HttpPost]
    public IActionResult EditOrderRoutePoint(OrderRoutePoint orderRoutePoint)
    {
        if (!ModelState.IsValid)
        {
            try
            {
                // Убедимся, что RoutePointId установлен правильно
                if (orderRoutePoint.RoutePointId == 0)
                {
                    ModelState.AddModelError(string.Empty, "Точка маршрута не выбрана.");
                    return View(orderRoutePoint);
                }

                // Проверка на существование записи OrderRoutePoint
                var existingOrderRoutePoint = _context.OrderRoutePoints
                    .FirstOrDefault(orp => orp.OrderId == orderRoutePoint.OrderId && orp.RoutePointId == orderRoutePoint.RoutePointId);

                if (existingOrderRoutePoint == null)
                {
                    ModelState.AddModelError(string.Empty, $"Связь между заказом с ID {orderRoutePoint.OrderId} и точкой маршрута с ID {orderRoutePoint.RoutePointId} не найдена.");
                    return View(orderRoutePoint);
                }

                // Обновляем существующую связь
                existingOrderRoutePoint.RoutePointId = orderRoutePoint.RoutePointId;
                _context.OrderRoutePoints.Update(existingOrderRoutePoint);
                _context.SaveChanges();

                // Перенаправляем пользователя к списку связей между заказами и точками маршрута
                return RedirectToAction("OrderRoutePoints", new { orderId = orderRoutePoint.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(orderRoutePoint);
            }
        }

        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", orderRoutePoint.RoutePointId);
        ViewBag.OrderId = orderRoutePoint.OrderId;
        return View(orderRoutePoint);
    }

    public IActionResult DeleteOrderRoutePoint(int orderId, int routePointId)
    {
        var orderRoutePoint = _context.OrderRoutePoints
            .Include(orp => orp.Order)
            .Include(orp => orp.RoutePoint)
            .FirstOrDefault(orp => orp.OrderId == orderId && orp.RoutePointId == routePointId);

        if (orderRoutePoint == null)
        {
            return NotFound();
        }

        ViewBag.OrderId = orderRoutePoint.OrderId;
        return View(orderRoutePoint);
    }

    [HttpPost, ActionName("DeleteOrderRoutePoint")]
    public async Task<IActionResult> DeleteOrderRoutePointConfirmed(int orderId, int routePointId)
    {
        var orderRoutePoint = await _context.OrderRoutePoints
            .FirstOrDefaultAsync(orp => orp.OrderId == orderId && orp.RoutePointId == routePointId);

        if (orderRoutePoint == null)
        {
            return NotFound();
        }

        _context.OrderRoutePoints.Remove(orderRoutePoint);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(OrderRoutePoints), new { orderId = orderRoutePoint.OrderId });
    }

    #endregion

    //#region Связь между заказами и точками маршрута

    //public IActionResult OrderRoutePoints(int orderId)
    //{
    //    var orderRoutePoints = _context.OrderRoutePoints
    //        .Include(orp => orp.Order)
    //        .Include(orp => orp.RoutePoint)
    //        .Where(orp => orp.OrderId == orderId)
    //        .ToList();

    //    ViewBag.OrderId = orderId;
    //    return View(orderRoutePoints);
    //}

    //public IActionResult AddOrderRoutePoint(int orderId)
    //{
    //    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name");
    //    ViewBag.OrderId = orderId;
    //    return View();
    //}

    //[HttpPost]
    //public IActionResult AddOrderRoutePoint(OrderRoutePoint orderRoutePoint)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            // Убедимся, что RoutePointId установлен правильно
    //            if (orderRoutePoint.RoutePointId == 0)
    //            {
    //                ModelState.AddModelError(string.Empty, "Точка маршрута не выбрана.");
    //                return View(orderRoutePoint);
    //            }

    //            // Добавляем новую связь
    //            _context.OrderRoutePoints.Add(orderRoutePoint);
    //            _context.SaveChanges();

    //            // Перенаправляем пользователя к списку связей между заказами и точками маршрута
    //            return RedirectToAction("OrderRoutePoints", new { orderId = orderRoutePoint.OrderId });
    //        }
    //        catch (Exception ex)
    //        {
    //            ModelState.AddModelError(string.Empty, ex.Message);
    //            return View(orderRoutePoint);
    //        }
    //    }

    //    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", orderRoutePoint.RoutePointId);
    //    ViewBag.OrderId = orderRoutePoint.OrderId;
    //    return View(orderRoutePoint);
    //}

    //public IActionResult EditOrderRoutePoint(int id)
    //{
    //    var orderRoutePoint = _context.OrderRoutePoints
    //        .Include(orp => orp.Order)
    //        .Include(orp => orp.RoutePoint)
    //        .FirstOrDefault(orp => orp.Id == id);

    //    if (orderRoutePoint == null)
    //    {
    //        return NotFound();
    //    }

    //    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", orderRoutePoint.RoutePointId);
    //    ViewBag.OrderId = orderRoutePoint.OrderId;
    //    return View(orderRoutePoint);
    //}

    //[HttpPost]
    //public IActionResult EditOrderRoutePoint(OrderRoutePoint orderRoutePoint)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            // Убедимся, что RoutePointId установлен правильно
    //            if (orderRoutePoint.RoutePointId == 0)
    //            {
    //                ModelState.AddModelError(string.Empty, "Точка маршрута не выбрана.");
    //                return View(orderRoutePoint);
    //            }

    //            _context.OrderRoutePoints.Update(orderRoutePoint);
    //            _context.SaveChanges();

    //            // Перенаправляем пользователя к списку связей между заказами и точками маршрута
    //            return RedirectToAction("OrderRoutePoints", new { orderId = orderRoutePoint.OrderId });
    //        }
    //        catch (Exception ex)
    //        {
    //            ModelState.AddModelError(string.Empty, ex.Message);
    //            return View(orderRoutePoint);
    //        }
    //    }

    //    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", orderRoutePoint.RoutePointId);
    //    ViewBag.OrderId = orderRoutePoint.OrderId;
    //    return View(orderRoutePoint);
    //}

    //public IActionResult DeleteOrderRoutePoint(int id)
    //{
    //    var orderRoutePoint = _context.OrderRoutePoints
    //        .Include(orp => orp.Order)
    //        .Include(orp => orp.RoutePoint)
    //        .FirstOrDefault(orp => orp.Id == id);

    //    if (orderRoutePoint == null)
    //    {
    //        return NotFound();
    //    }

    //    ViewBag.OrderId = orderRoutePoint.OrderId;
    //    return View(orderRoutePoint);
    //}

    //[HttpPost, ActionName("DeleteOrderRoutePoint")]
    //public async Task<IActionResult> DeleteOrderRoutePointConfirmed(int id)
    //{
    //    var orderRoutePoint = await _context.OrderRoutePoints.FindAsync(id);
    //    if (orderRoutePoint == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.OrderRoutePoints.Remove(orderRoutePoint);
    //    await _context.SaveChangesAsync();
    //    return RedirectToAction(nameof(OrderRoutePoints), new { orderId = orderRoutePoint.OrderId });
    //}

    //#endregion

    #endregion

    #region Методы управления схемами загрузки и продукции на этих схемах

    #region LoadingSchemes

    public IActionResult LoadingSchemes()
    {
        var loadingSchemes = _context.LoadingSchemes
            .Include(ls => ls.VehicleType)
            .Include(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .ThenInclude(p => p.PackagingTypes)
            .ToList();
        return View(loadingSchemes);
    }

    public IActionResult AddLoadingScheme()
    {
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
        ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name");
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult AddLoadingScheme(LoadingScheme loadingScheme)
    {
        if (!ModelState.IsValid)
        {
            //try
            //{
            // Преобразуем дату загрузки в UTC
            loadingScheme.LoadingDate = loadingScheme.LoadingDate.ToUniversalTime();

            // Добавляем схему погрузки в контекст
            _context.LoadingSchemes.Add(loadingScheme);
            _context.SaveChanges();


            // Перенаправляем пользователя к списку продуктов в новой схеме погрузки
            return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingScheme.Id });
           // }
           // catch (Exception ex)
           // {
                //ModelState.AddModelError(string.Empty, ex.Message);
           // }
        }

        // Если модель невалидна, повторно заполняем ViewData и возвращаем представление с ошибками
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
        ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name", loadingScheme.VehicleId);
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", loadingScheme.RouteId);
        return View(loadingScheme);
    }
    public IActionResult EditLoadingScheme(int id)
    {
        var loadingScheme = _context.LoadingSchemes
            .Include(ls => ls.VehicleType)
            .Include(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .FirstOrDefault(ls => ls.Id == id);

        if (loadingScheme == null)
        {
            return NotFound();
        }

        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
        ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name", loadingScheme.VehicleId);
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", loadingScheme.RouteId);
        return View(loadingScheme);
    }

    public IActionResult LoadingSchemeDetails(int id)
    {
        var loadingScheme = _context.LoadingSchemes
            .Include(ls => ls.VehicleType)
            .Include(ls => ls.LoadingProducts)
            .ThenInclude(lp => lp.Product)
            .ThenInclude(p => p.PackagingTypes)
            .FirstOrDefault(ls => ls.Id == id);

        if (loadingScheme == null)
        {
            return NotFound();
        }

        ViewBag.LoadingSchemeId = id;
        ViewBag.LoadingScheme = loadingScheme;

        return View(loadingScheme.LoadingProducts);
    }

    [HttpGet]
    public JsonResult GetLoadingSchemeDetails(int id)
    {
        try
        {
            var loadingScheme = _context.LoadingSchemes
                .Include(ls => ls.VehicleType)
                .Include(ls => ls.LoadingProducts)
                .ThenInclude(lp => lp.Product)
                .ThenInclude(p => p.PackagingTypes)
                .FirstOrDefault(ls => ls.Id == id);

            if (loadingScheme != null)
            {
                var result = new
                {
                    id = loadingScheme.Id,
                    name = loadingScheme.VehicleType.Name,
                    vehicleType = new
                    {
                        TrailerLength = loadingScheme.VehicleType.Length,
                        TrailerWidth = loadingScheme.VehicleType.Width
                    },
                    loadingProducts = loadingScheme.LoadingProducts.Select(lp => new LoadingProductDto
                    {
                        Id = lp.Id,
                        ProductId = lp.ProductId,
                        Quantity = lp.Quantity,
                        PositionX = lp.PositionX,
                        PositionY = lp.PositionY,
                        PositionZ = lp.PositionZ,
                        Product = new ProductDto
                        {
                            Id = lp.Product.Id,
                            Name = lp.Product.Name
                        },
                        PackagingType = lp.Product.PackagingTypes.FirstOrDefault() != null ? new PackagingTypeDto
                        {
                            Id = lp.Product.PackagingTypes.FirstOrDefault().Id,
                            Name = lp.Product.PackagingTypes.FirstOrDefault().Name,
                            Length = lp.Product.PackagingTypes.FirstOrDefault().Length,
                            Width = lp.Product.PackagingTypes.FirstOrDefault().Width,
                            Height = lp.Product.PackagingTypes.FirstOrDefault().Height,
                            Weight = lp.Product.PackagingTypes.FirstOrDefault().Weight
                        } : null
                    }).ToList()
                };

                // Логирование результатов
                Debug.WriteLine($"LoadingSchemeId: {result.id}");
                Debug.WriteLine($"VehicleType: {result.vehicleType.TrailerLength} x {result.vehicleType.TrailerWidth}");
                result.loadingProducts.ForEach(lp =>
                {
                    Debug.WriteLine($"LoadingProductId: {lp.Id}, Product: {lp.Product.Name}, PackagingType: {lp.PackagingType?.Name ?? "Not found"}");
                });

                // Дополнительная проверка на сервере
                foreach (var lp in result.loadingProducts)
                {
                    if (lp.PackagingType == null)
                    {
                        Debug.WriteLine($"Продукт с ID {lp.Id} не имеет связанного типа упаковки");
                    }
                }

                return Json(result);
            }

            return Json(new { success = false, message = "Схема погрузки не найдена" });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при получении данных о схеме погрузки: {ex.Message}");
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult EditLoadingScheme(LoadingScheme loadingScheme)
    {
        if (!ModelState.IsValid)
        {
            _context.LoadingSchemes.Update(loadingScheme);
            _context.SaveChanges();
            return RedirectToAction("LoadingSchemes");
        }

        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", loadingScheme.VehicleTypeId);
        ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Name", loadingScheme.VehicleId);
        ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Name", loadingScheme.RouteId);
        return View(loadingScheme);
    }

    #endregion

    #region LoadingProducts

    public IActionResult LoadingProducts(int loadingSchemeId)
    {
        var loadingProducts = _context.LoadingProducts
            .Include(lp => lp.Product)
            .ThenInclude(p => p.PackagingTypes)
            .Include(lp => lp.LoadingScheme)
            .Where(lp => lp.LoadingSchemeId == loadingSchemeId)
            .ToList();

        ViewBag.LoadingSchemeId = loadingSchemeId;
        ViewBag.LoadingScheme = _context.LoadingSchemes
            .Include(ls => ls.VehicleType)
            .FirstOrDefault(ls => ls.Id == loadingSchemeId);

        return View(loadingProducts);
    }

    public IActionResult AddLoadingProduct(int loadingSchemeId)
    {
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name");
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name");
        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name");
        ViewBag.LoadingSchemeId = loadingSchemeId;
        return View();
    }

    [HttpPost]
    public IActionResult AddLoadingProduct(LoadingProduct loadingProduct)
    {
        if (!ModelState.IsValid)
        {
            try
            {
                // Проверяем существование связанного продукта
                var product = _context.Products
                    .Include(p => p.PackagingTypes)
                    .FirstOrDefault(p => p.Id == loadingProduct.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Выбранный продукт не существует");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Проверяем существование связанного типа упаковки
                var packagingType = product.PackagingTypes
                    .FirstOrDefault(pt => pt.Id == loadingProduct.PackagingTypeId);
                if (packagingType == null)
                {
                    ModelState.AddModelError("PackagingTypeId", "Выбранный тип упаковки не существует для данного продукта");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Проверяем существование связанного заказа
                var order = _context.Orders.Find(loadingProduct.OrderId);
                if (order == null)
                {
                    ModelState.AddModelError("OrderId", "Выбранный заказ не существует");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Проверяем существование связанной точки маршрута
                var routePoint = _context.RoutePoints.Find(loadingProduct.RoutePointId);
                if (routePoint == null)
                {
                    ModelState.AddModelError("RoutePointId", "Выбранная точка маршрута не существует");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Добавляем новый продукт в схему погрузки
                _context.LoadingProducts.Add(loadingProduct);
                _context.SaveChanges();
                return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                return View(loadingProduct);
            }
        }

        // Если модель невалидна, повторно заполняем ViewData и возвращаем представление с ошибками
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
        return View(loadingProduct);
    }
    public IActionResult EditLoadingProduct(int id)
    {
        var loadingProduct = _context.LoadingProducts
            .Include(lp => lp.Product)
            .ThenInclude(p => p.PackagingTypes)
            .Include(lp => lp.LoadingScheme)
            .Include(lp => lp.Order)
            .Include(lp => lp.RoutePoint)
            .FirstOrDefault(lp => lp.Id == id);

        if (loadingProduct == null)
        {
            return NotFound();
        }

        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
        return View(loadingProduct);
    }

    [HttpPost]
    public IActionResult EditLoadingProduct(LoadingProduct loadingProduct)
    {
        if (!ModelState.IsValid)
        {
            try
            {
                // Проверяем существование связанного продукта
                var product = _context.Products
                    .Include(p => p.PackagingTypes)
                    .FirstOrDefault(p => p.Id == loadingProduct.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Выбранный продукт не существует");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Проверяем существование связанного типа упаковки
                var packagingType = product.PackagingTypes
                    .FirstOrDefault(pt => pt.Id == loadingProduct.PackagingTypeId);
                if (packagingType == null)
                {
                    ModelState.AddModelError("PackagingTypeId", "Выбранный тип упаковки не существует для данного продукта");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Проверяем существование связанного заказа
                var order = _context.Orders.Find(loadingProduct.OrderId);
                if (order == null)
                {
                    ModelState.AddModelError("OrderId", "Выбранный заказ не существует");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Проверяем существование связанной точки маршрута
                var routePoint = _context.RoutePoints.Find(loadingProduct.RoutePointId);
                if (routePoint == null)
                {
                    ModelState.AddModelError("RoutePointId", "Выбранная точка маршрута не существует");
                    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                    ViewData["PackagingTypeId"] = new SelectList(product.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                    ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                    ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                    ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                    return View(loadingProduct);
                }

                // Обновляем продукт в схеме погрузки
                _context.LoadingProducts.Update(loadingProduct);
                _context.SaveChanges();
                return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
                ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
                ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
                ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
                ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
                return View(loadingProduct);
            }
        }

        // Если модель невалидна, повторно заполняем ViewData и возвращаем представление с ошибками
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", loadingProduct.ProductId);
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", loadingProduct.PackagingTypeId);
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Name", loadingProduct.OrderId);
        ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", loadingProduct.RoutePointId);
        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
        return View(loadingProduct);
    }
    public IActionResult DeleteLoadingProduct(int id)
    {
        var loadingProduct = _context.LoadingProducts
            .Include(lp => lp.Product)
            .ThenInclude(p => p.PackagingTypes)
            .Include(lp => lp.LoadingScheme)
            .Include(lp => lp.Order)
            .Include(lp => lp.RoutePoint)
            .FirstOrDefault(lp => lp.Id == id);

        if (loadingProduct == null)
        {
            return NotFound();
        }

        ViewBag.LoadingSchemeId = loadingProduct.LoadingSchemeId;
        return View(loadingProduct);
    }

    [HttpPost, ActionName("DeleteLoadingProduct")]
    public async Task<IActionResult> DeleteLoadingProductConfirmed(int id)
    {
        var loadingProduct = await _context.LoadingProducts
            .FirstOrDefaultAsync(lp => lp.Id == id);

        if (loadingProduct == null)
        {
            return NotFound();
        }

        _context.LoadingProducts.Remove(loadingProduct);
        await _context.SaveChangesAsync();
        return RedirectToAction("LoadingProducts", new { loadingSchemeId = loadingProduct.LoadingSchemeId });
    }
    #endregion
    [HttpGet]
    public JsonResult GetProductDetails(int id)
    {
        try
        {
            var product = _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                return Json(new
                {
                    id = product.Id,
                    name = product.Name,
                    packagingTypes = product.PackagingTypes.Select(pt => new
                    {
                        id = pt.Id,
                        name = pt.Name
                    }).ToList()
                });
            }

            return Json(new { success = false, message = "Продукт не найден" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public JsonResult GetProductPackagingDetails(int id)
    {
        var packagingType = _context.PackagingTypes
            .FirstOrDefault(pt => pt.Id == id);

        if (packagingType != null)
        {
            return Json(new
            {
                id = packagingType.Id,
                name = packagingType.Name,
                length = packagingType.Length,
                width = packagingType.Width,
                height = packagingType.Height,
                weight = packagingType.Weight
            });
        }

        return Json(null);
    }

    #endregion
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
