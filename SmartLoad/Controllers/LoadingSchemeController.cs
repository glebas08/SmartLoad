using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Data;
using SmartLoad.Models;
using SmartLoad.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLoad.Controllers
{
    public class LoadingSchemeController : Controller
    {
    //    private readonly ApplicationDbContext _context;
    //    private readonly LoadingService _loadingService;

    //    public LoadingSchemeController(ApplicationDbContext context, LoadingService loadingService)
    //    {
    //        _context = context;
    //        _loadingService = loadingService;
    //    }

    //    // GET: LoadingScheme/Create
    //    public IActionResult Create()
    //    {
    //        var viewModel = new LoadingSchemeCreateViewModel
    //        {
    //            Vehicles = _context.Vehicles
    //                .Include(v => v.VehicleType)
    //                .ToList(),
    //            Routes = _context.Routes
    //                .Where(r => r.Status != "Завершен")
    //                .ToList()
    //        };

    //        return View(viewModel);
    //    }

    //    // POST: LoadingScheme/Create
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public IActionResult Create(LoadingSchemeCreateViewModel viewModel)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            return RedirectToAction("Calculate", new { vehicleId = viewModel.SelectedVehicleId, routeId = viewModel.SelectedRouteId });
    //        }

    //        // Если модель невалидна, заполняем списки снова
    //        viewModel.Vehicles = _context.Vehicles
    //            .Include(v => v.VehicleType)
    //            .ToList();
    //        viewModel.Routes = _context.Routes
    //            .Where(r => r.Status != "Завершен")
    //            .ToList();

    //        return View(viewModel);
    //    }

    //    // GET: LoadingScheme/Calculate
    //    public IActionResult Calculate(int vehicleId, int routeId)
    //    {
    //        var vehicle = _context.Vehicles
    //            .Include(v => v.VehicleType)
    //            .FirstOrDefault(v => v.Id == vehicleId);

    //        if (vehicle == null)
    //        {
    //            return NotFound("Транспортное средство не найдено");
    //        }

    //        var route = _context.Routes
    //            .Include(r => r.RoutePointMappings)
    //                .ThenInclude(rpm => rpm.RoutePoint)
    //            .FirstOrDefault(r => r.Id == routeId);

    //        if (route == null)
    //        {
    //            return NotFound("Маршрут не найден");
    //        }

    //        // Получаем все заказы для точек маршрута
    //        var routePointIds = route.RoutePointMappings.Select(rpm => rpm.RoutePointId).ToList();
    //        var orders = _context.Orders
    //            .Include(o => o.OrderProducts)
    //                .ThenInclude(op => op.Product)
    //            .Where(o => o.RoutePointId.HasValue && routePointIds.Contains(o.RoutePointId.Value))
    //            .ToList();

    //        try
    //        {
    //            // Используем сервис для расчета размещения груза
    //            var placements = _loadingService.CalculatePlacement(vehicle.VehicleType, route, orders);

    //            // Создаем модель представления с результатами
    //            var resultViewModel = new LoadingSchemeResultViewModel
    //            {
    //                Vehicle = vehicle,
    //                Route = route,
    //                Orders = orders,
    //                Placements = placements,
    //                // Здесь можно добавить расчет нагрузки на оси
    //                AxleLoads = CalculateAxleLoads(vehicle, placements, orders)
    //            };

    //            return View(resultViewModel);
    //        }
    //        catch (InvalidOperationException ex)
    //        {
    //            // Обработка ошибок при расчете размещения
    //            ModelState.AddModelError("", ex.Message);
    //            return RedirectToAction("Create");
    //        }
    //    }

    //    // Метод для расчета нагрузки на оси
    //    private Dictionary<string, double> CalculateAxleLoads(Vehicle vehicle, List<CargoPlacement> placements, List<Order> orders)
    //    {
    //        // Здесь реализуем алгоритм расчета нагрузки на оси
    //        // Это упрощенная версия, в реальности алгоритм будет сложнее
    //        var axleLoads = new Dictionary<string, double>();

    //        // Получаем общий вес груза
    //        double totalWeight = 0;
    //        foreach (var placement in placements)
    //        {
    //            var product = orders
    //                .SelectMany(o => o.OrderProducts)
    //                .FirstOrDefault(op => op.Product.Id == placement.ProductId)?.Product;

    //            if (product != null)
    //            {
    //                totalWeight += product.Weight;
    //            }
    //        }

    //        // Распределяем вес по осям (упрощенно)
    //        // Передняя ось тягача
    //        axleLoads["Передняя ось тягача"] = vehicle.EmptyFrontAxleLoad + (totalWeight * 0.2);

    //        // Задняя ось тягача
    //        axleLoads["Задняя ось тягача"] = vehicle.EmptyRearAxleLoad + (totalWeight * 0.3);

    //        // Оси полуприцепа (предполагаем, что их 3)
    //        double trailerAxleLoad = (totalWeight * 0.5) / 3;
    //        axleLoads["Ось полуприцепа 1"] = trailerAxleLoad;
    //        axleLoads["Ось полуприцепа 2"] = trailerAxleLoad;
    //        axleLoads["Ось полуприцепа 3"] = trailerAxleLoad;

    //        return axleLoads;
    //    }

    //    // GET: LoadingScheme/SaveScheme
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> SaveScheme(LoadingSchemeResultViewModel viewModel)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            // Создаем новую схему загрузки
    //            var loadingScheme = new LoadingScheme
    //            {
    //                VehicleId = viewModel.Vehicle.Id,
    //                RouteId = viewModel.Route.Id,
    //                LoadingDate = DateTime.Now,
    //                Status = "Создана",
    //                Notes = $"Схема загрузки для маршрута {viewModel.Route.Name}",
    //                LoadingProducts = new List<LoadingProduct>()
    //            };

    //            // Добавляем продукты в схему загрузки
    //            foreach (var placement in viewModel.Placements)
    //            {
    //                var product = viewModel.Orders
    //                    .SelectMany(o => o.OrderProducts)
    //                    .FirstOrDefault(op => op.Product.Id == placement.ProductId)?.Product;

    //                if (product != null)
    //                {
    //                    var order = viewModel.Orders
    //                        .FirstOrDefault(o => o.OrderProducts.Any(op => op.Product.Id == placement.ProductId));

    //                    var loadingProduct = new LoadingProduct
    //                    {
    //                        ProductId = placement.ProductId,
    //                        OrderId = order?.Id ?? 0,
    //                        RoutePointId = order?.RoutePointId ?? 0,
    //                        Quantity = 1,
    //                        PositionX = (float)placement.X,
    //                        PositionY = (float)placement.Y,
    //                        PositionZ = (float)placement.Z
    //                    };

    //                    loadingScheme.LoadingProducts.Add(loadingProduct);
    //                }
    //            }

    //            _context.LoadingSchemes.Add(loadingScheme);
    //            await _context.SaveChangesAsync();

    //            return RedirectToAction("Details", "LoadingScheme", new { id = loadingScheme.Id });
    //        }

    //        return View("Calculate", viewModel);
    //    }

    //    // GET: LoadingScheme/Details/5
    //    public async Task<IActionResult> Details(int id)
    //    {
    //        var loadingScheme = await _context.LoadingSchemes
    //            .Include(ls => ls.Vehicle)
    //                .ThenInclude(v => v.VehicleType)
    //            .Include(ls => ls.Route)
    //            .Include(ls => ls.LoadingProducts)
    //                .ThenInclude(lp => lp.Product)
    //            .Include(ls => ls.LoadingProducts)
    //                .ThenInclude(lp => lp.RoutePoint)
    //            .FirstOrDefaultAsync(ls => ls.Id == id);

    //        if (loadingScheme == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(loadingScheme);
    //    }
    }
}
