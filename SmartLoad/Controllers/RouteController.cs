using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;
using SmartLoad.Data;
using System.Linq;

namespace SmartLoad.Controllers
{
    public class RouteController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        #region Маршруты
        #region по которому будут доставлятся заказы
        public IActionResult Routes()
        {
            var routes = _context.Routes
                .Include(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .ToList();

            return View(routes);
        }

        public IActionResult AddRoute()
        {
            ViewBag.AvailableRoutePoints = _context.RoutePoints.ToList();
            return View(new Rout { RoutePointMappings = new List<RoutePointMapping>() });
        }

        [HttpGet]
        public IActionResult GetOrderByRoutePoint(int routePointId)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.RoutePointId == routePointId);

            if (order == null)
            {
                return Json(new { name = "Нет заказа", id = 0 });
            }

            return Json(new { name = order.Name, id = order.Id });
        }

        [HttpPost]
        public IActionResult AddRoute(Rout routeModel, int[] selectedRoutePointIds, int[] orderInRoute)
        {
            if (ModelState.IsValid)
            {
                // Преобразование дат в UTC
                routeModel.DepartureDate = DateTime.SpecifyKind(routeModel.DepartureDate, DateTimeKind.Utc);
                routeModel.ArrivalDate = DateTime.SpecifyKind(routeModel.ArrivalDate, DateTimeKind.Utc);

                // Сначала сохраняем маршрут, чтобы получить его Id
                _context.Routes.Add(routeModel);
                _context.SaveChanges();

                // Добавляем точки маршрута через промежуточную таблицу
                if (selectedRoutePointIds != null && selectedRoutePointIds.Length > 0)
                {
                    for (int i = 0; i < selectedRoutePointIds.Length; i++)
                    {
                        var routePointId = selectedRoutePointIds[i];
                        var order = (orderInRoute != null && i < orderInRoute.Length) ? orderInRoute[i] : i + 1;

                        var routePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == routePointId);
                        if (routePoint != null)
                        {
                            var mapping = new RoutePointMapping
                            {
                                RouteId = routeModel.Id,
                                RoutePointId = routePointId,
                                OrderInRoute = order,
                                EstimatedArrivalTime = routePoint.UnloadingDate
                            };
                            _context.RoutePointMappings.Add(mapping);
                        }
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("Routes");
            }

            ViewBag.AvailableRoutePoints = _context.RoutePoints.ToList();
            return View(routeModel);
        }



        public IActionResult EditRoute(int id)
        {
            // Изменение: Включаем RoutePointMappings вместо RoutePoints
            var route = _context.Routes
                .Include(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .FirstOrDefault(r => r.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            // Изменение: Получаем доступные точки, которые еще не добавлены к маршруту
            ViewBag.AvailableRoutePoints = _context.RoutePoints
                .Where(rp => !route.RoutePointMappings.Any(rpm => rpm.RoutePointId == rp.Id))
                .ToList();

            return View(route);
        }

        [HttpPost]
        public IActionResult EditRoute(Rout route, int[] selectedRoutePointIds, int[] orderInRoute) // Изменение: добавлены параметры
        {
            if (ModelState.IsValid)
            {
                // Обновляем основные данные маршрута
                _context.Routes.Update(route);

                // Изменение: Получаем текущие маппинги
                var existingMappings = _context.RoutePointMappings
                    .Where(rpm => rpm.RouteId == route.Id)
                    .ToList();

                // Удаляем все существующие маппинги
                _context.RoutePointMappings.RemoveRange(existingMappings);
                _context.SaveChanges();

                // Создаем новые маппинги
                if (selectedRoutePointIds != null && orderInRoute != null &&
                    selectedRoutePointIds.Length == orderInRoute.Length)
                {
                    for (int i = 0; i < selectedRoutePointIds.Length; i++)
                    {
                        var routePointId = selectedRoutePointIds[i];
                        var order = orderInRoute[i];

                        // Проверяем, существует ли точка маршрута
                        var existingRoutePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == routePointId);
                        if (existingRoutePoint != null)
                        {
                            // Создаем запись в промежуточной таблице
                            var mapping = new RoutePointMapping
                            {
                                RouteId = route.Id,
                                RoutePointId = routePointId,
                                OrderInRoute = order,
                                EstimatedArrivalTime = existingRoutePoint.UnloadingDate
                            };
                            _context.RoutePointMappings.Add(mapping);
                        }
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("Routes");
            }

            // Если модель невалидна, заново загружаем данные
            var routeWithMappings = _context.Routes
                .Include(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .FirstOrDefault(r => r.Id == route.Id);

            ViewBag.AvailableRoutePoints = _context.RoutePoints
                .Where(rp => !routeWithMappings.RoutePointMappings.Any(rpm => rpm.RoutePointId == rp.Id))
                .ToList();

            return View(routeWithMappings);
        }

        public IActionResult DeleteRoute(int id)
        {
            // Изменение: Включаем RoutePointMappings вместо RoutePoints
            var route = _context.Routes
                .Include(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .FirstOrDefault(r => r.Id == id);

            if (route == null)
            {
                Console.WriteLine("Route not found");
                return NotFound();
            }

            Console.WriteLine($" ----------ПРОВЕРКА Route loaded: {route.Name}");
            return View(route);
        }

        [HttpPost, ActionName("DeleteRoute")]
        public async Task<IActionResult> DeleteRouteConfirmed(int id)
        {
            // Изменение: Включаем RoutePointMappings
            var route = await _context.Routes
                .Include(r => r.RoutePointMappings)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            // Изменение: Удаляем все связанные маппинги
            _context.RoutePointMappings.RemoveRange(route.RoutePointMappings);

            // Удаляем маршрут
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Routes));
        }

        #endregion

        #region Точки маршрута

        // Новый метод: Просмотр точек маршрута
        public IActionResult RoutePoints(int routeId)
        {
            var route = _context.Routes
                .Include(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }

            // Сортируем точки по порядку в маршруте
            var orderedPoints = route.RoutePointMappings
                .OrderBy(rpm => rpm.OrderInRoute)
                .ToList();

            ViewBag.RouteName = route.Name;
            ViewBag.RouteId = routeId;

            return View(orderedPoints);
        }
        // Новый метод: Добавление точки к маршруту
        public IActionResult AddRoutePointToRoute(int routeId)
        {
            var routeModel = _context.Routes.FirstOrDefault(r => r.Id == routeId);
            if (routeModel == null)
            {
                return NotFound();
            }

            // Получаем точки, которые еще не добавлены к маршруту
            var existingMappings = _context.RoutePointMappings
                .Where(rpm => rpm.RouteId == routeId)
                .Select(rpm => rpm.RoutePointId)
                .ToList();

            var availableRoutePoints = _context.RoutePoints
                .Where(rp => !existingMappings.Contains(rp.Id))
                .ToList();

            ViewBag.RouteId = routeId;
            ViewBag.RouteName = routeModel.Name;
            ViewBag.AvailableRoutePoints = new SelectList(availableRoutePoints, "Id", "Name");

            // Определяем следующий порядковый номер
            int nextOrder = 1;
            if (existingMappings.Any())
            {
                nextOrder = _context.RoutePointMappings
                    .Where(rpm => rpm.RouteId == routeId)
                    .Max(rpm => rpm.OrderInRoute) + 1;
            }

            ViewBag.NextOrder = nextOrder;

            return View(new RoutePointMapping { RouteId = routeId });
        }

        // Новый метод: Сохранение новой точки маршрута
        [HttpPost]
        public IActionResult AddRoutePointToRoute(RoutePointMapping mapping)
        {
            if (ModelState.IsValid)
            {
                // Проверяем, существует ли уже такая связь
                var existingMapping = _context.RoutePointMappings
                    .FirstOrDefault(rpm => rpm.RouteId == mapping.RouteId && rpm.RoutePointId == mapping.RoutePointId);
                if (existingMapping != null)
                {
                    ModelState.AddModelError("", "Эта точка уже добавлена к маршруту");
                    var currentRoute = _context.Routes.FirstOrDefault(r => r.Id == mapping.RouteId);
                    var availableRoutePoints = _context.RoutePoints
                        .Where(rp => !_context.RoutePointMappings
                            .Where(rpm => rpm.RouteId == mapping.RouteId)
                            .Select(rpm => rpm.RoutePointId)
                            .Contains(rp.Id))
                        .ToList();

                    ViewBag.RouteId = mapping.RouteId;
                    ViewBag.RouteName = currentRoute?.Name;
                    ViewBag.AvailableRoutePoints = new SelectList(availableRoutePoints, "Id", "Name");
                    return View(mapping);
                }

                // Если EstimatedArrivalTime не указано, берем дату из RoutePoint
                if (!mapping.EstimatedArrivalTime.HasValue)
                {
                    var routePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == mapping.RoutePointId);
                    if (routePoint != null)
                    {
                        mapping.EstimatedArrivalTime = routePoint.UnloadingDate;
                    }
                }

                _context.RoutePointMappings.Add(mapping);
                _context.SaveChanges();

                return RedirectToAction("RoutePoints", new { routeId = mapping.RouteId });
            }

            var currentRouteModel = _context.Routes.FirstOrDefault(r => r.Id == mapping.RouteId);
            var availablePoints = _context.RoutePoints
                .Where(rp => !_context.RoutePointMappings
                    .Where(rpm => rpm.RouteId == mapping.RouteId)
                    .Select(rpm => rpm.RoutePointId)
                    .Contains(rp.Id))
                .ToList();

            ViewBag.RouteId = mapping.RouteId;
            ViewBag.RouteName = currentRouteModel?.Name;
            ViewBag.AvailableRoutePoints = new SelectList(availablePoints, "Id", "Name");

            return View(mapping);
        }
        // Новый метод: Сохранение изменений связи маршрут-точка
        [HttpPost]
        public IActionResult EditRoutePointMapping(RoutePointMapping mapping)
        {
            if (ModelState.IsValid)
            {
                _context.RoutePointMappings.Update(mapping);
                _context.SaveChanges();

                return RedirectToAction("RoutePoints", new { routeId = mapping.RouteId });
            }

            var routePointMapping = _context.RoutePointMappings
                .Include(rpm => rpm.RoutePoint)
                .Include(rpm => rpm.Route)
                .FirstOrDefault(rpm => rpm.Id == mapping.Id);

            ViewBag.RouteId = mapping.RouteId;
            ViewBag.RouteName = routePointMapping?.Route.Name;
            ViewBag.RoutePointName = routePointMapping?.RoutePoint.Name;

            return View(mapping);
        }

        // Новый метод: Удаление связи маршрут-точка
        public IActionResult DeleteRoutePointMapping(int id)
        {
            var mapping = _context.RoutePointMappings
                .Include(rpm => rpm.RoutePoint)
                .Include(rpm => rpm.Route)
                .FirstOrDefault(rpm => rpm.Id == id);

            if (mapping == null)
            {
                return NotFound();
            }

            ViewBag.RouteId = mapping.RouteId;
            ViewBag.RouteName = mapping.Route.Name;
            ViewBag.RoutePointName = mapping.RoutePoint.Name;

            return View(mapping);
        }

        // Новый метод: Подтверждение удаления связи маршрут-точка
        [HttpPost, ActionName("DeleteRoutePointMapping")]
        public async Task<IActionResult> DeleteRoutePointMappingConfirmed(int id)
        {
            var mapping = await _context.RoutePointMappings
                .FirstOrDefaultAsync(rpm => rpm.Id == id);

            if (mapping == null)
            {
                return NotFound();
            }

            int routeId = mapping.RouteId;
            _context.RoutePointMappings.Remove(mapping);
            await _context.SaveChangesAsync();

            // Перенумеруем оставшиеся точки маршрута
            var remainingMappings = await _context.RoutePointMappings
                .Where(rpm => rpm.RouteId == routeId)
                .OrderBy(rpm => rpm.OrderInRoute)
                .ToListAsync();

            for (int i = 0; i < remainingMappings.Count; i++)
            {
                remainingMappings[i].OrderInRoute = i + 1;
                _context.RoutePointMappings.Update(remainingMappings[i]);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("RoutePoints", new { routeId });
        }

        // Новый метод: Изменение порядка точек маршрута (перемещение вверх)
        public async Task<IActionResult> MoveRoutePointUp(int id)
        {
            var mapping = await _context.RoutePointMappings
                .FirstOrDefaultAsync(rpm => rpm.Id == id);

            if (mapping == null)
            {
                return NotFound();
            }

            // Находим предыдущую точку в порядке
            var previousMapping = await _context.RoutePointMappings
                .Where(rpm => rpm.RouteId == mapping.RouteId && rpm.OrderInRoute < mapping.OrderInRoute)
                .OrderByDescending(rpm => rpm.OrderInRoute)
                .FirstOrDefaultAsync();

            if (previousMapping != null)
            {
                // Меняем порядок
                int tempOrder = mapping.OrderInRoute;
                mapping.OrderInRoute = previousMapping.OrderInRoute;
                previousMapping.OrderInRoute = tempOrder;

                _context.RoutePointMappings.Update(mapping);
                _context.RoutePointMappings.Update(previousMapping);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("RoutePoints", new { routeId = mapping.RouteId });
        }

        // Новый метод: Изменение порядка точек маршрута (перемещение вниз)
        public async Task<IActionResult> MoveRoutePointDown(int id)
        {
            var mapping = await _context.RoutePointMappings
                .FirstOrDefaultAsync(rpm => rpm.Id == id);

            if (mapping == null)
            {
                return NotFound();
            }

            // Находим следующую точку в порядке
            var nextMapping = await _context.RoutePointMappings
                .Where(rpm => rpm.RouteId == mapping.RouteId && rpm.OrderInRoute > mapping.OrderInRoute)
                .OrderBy(rpm => rpm.OrderInRoute)
                .FirstOrDefaultAsync();

            if (nextMapping != null)
            {
                // Меняем порядок
                int tempOrder = mapping.OrderInRoute;
                mapping.OrderInRoute = nextMapping.OrderInRoute;
                nextMapping.OrderInRoute = tempOrder;

                _context.RoutePointMappings.Update(mapping);
                _context.RoutePointMappings.Update(nextMapping);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("RoutePoints", new { routeId = mapping.RouteId });
        }

        #endregion

        #endregion


        public IActionResult Details(int routeId)
        {
            var route = _context.Routes
                .Include(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }

            // Получаем заказы для каждой точки маршрута
            var routePointIds = route.RoutePointMappings.Select(rpm => rpm.RoutePointId).ToList();
            var orders = _context.Orders
                .Where(o => o.RoutePointId.HasValue && routePointIds.Contains(o.RoutePointId.Value))
                .ToList();

            // Группируем заказы по точкам маршрута
            var ordersByRoutePoint = new Dictionary<int, List<string>>();
            foreach (var order in orders)
            {
                if (order.RoutePointId.HasValue) // Проверяем, что RoutePointId имеет значение
                {
                    int routePointId = order.RoutePointId.Value; // Получаем значение
                    if (!ordersByRoutePoint.ContainsKey(routePointId))
                    {
                        ordersByRoutePoint[routePointId] = new List<string>();
                    }
                    ordersByRoutePoint[routePointId].Add(order.Name);
                }
            }


            ViewBag.Orders = ordersByRoutePoint;

            return View(route);
        }




        //метод для получения продуктов по id заказа
        [HttpGet]
        public IActionResult GetOrderProducts(int orderId)
        {
            var products = _context.OrderProducts
                .Where(op => op.OrderId == orderId)
                .Include(op => op.Product)
                .Select(op => new { op.Product.Id, op.Product.Name })
                .ToList();

            ViewBag.Orders = _context.Orders.ToList(); // Инициализация списка заказов
            return Json(products);
        }

        // Новый метод: Получение заказов для точки маршрута
        [HttpGet]
        public IActionResult GetOrdersForRoutePoint(int routePointId)
        {
            var orders = _context.Orders
                .Where(o => o.RoutePointId == routePointId)
                .Select(o => new { o.Id, o.Name })
                .ToList();

            return Json(orders);
        }

        // Новый метод: Получение точек маршрута в порядке следования
        [HttpGet]
        public IActionResult GetOrderedRoutePoints(int routeId)
        {
            var orderedPoints = _context.RoutePointMappings
                .Where(rpm => rpm.RouteId == routeId)
                .OrderBy(rpm => rpm.OrderInRoute)
                .Include(rpm => rpm.RoutePoint)
                .Select(rpm => new {
                    rpm.RoutePoint.Id,
                    rpm.RoutePoint.Name,
                    OrderInRoute = rpm.OrderInRoute,
                    EstimatedArrival = rpm.EstimatedArrivalTime
                })
                .ToList();

            return Json(orderedPoints);
        }
    }
}



#region предыдущий метод, в котормо мы отделяем точки маршрута, сохраняем 
/*
[HttpPost]
        public IActionResult AddRoute(Rout route, int[] selectedRoutePointIds, int[] orderInRoute) // Изменение: добавлены параметры для точек маршрута
{
    if (ModelState.IsValid)
    {
        Преобразование дат в UTC
        route.DepartureDate = DateTime.SpecifyKind(route.DepartureDate, DateTimeKind.Utc);
        route.ArrivalDate = DateTime.SpecifyKind(route.ArrivalDate, DateTimeKind.Utc);

        Сначала сохраняем маршрут, чтобы получить его Id
        _context.Routes.Add(route);
        _context.SaveChanges();

    Изменение: Создаем записи в промежуточной таблице RoutePointMapping
        if (selectedRoutePointIds != null && orderInRoute != null &&
            selectedRoutePointIds.Length == orderInRoute.Length)
        {
            for (int i = 0; i < selectedRoutePointIds.Length; i++)
            {
                var routePointId = selectedRoutePointIds[i];
                var order = orderInRoute[i];

                Проверяем, существует ли точка маршрута
                var existingRoutePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == routePointId);
                if (existingRoutePoint != null)
                {
                    Создаем запись в промежуточной таблице
                   var mapping = new RoutePointMapping
                   {
                       RouteId = route.Id,
                       RoutePointId = routePointId,
                       OrderInRoute = order,
                       EstimatedArrivalTime = existingRoutePoint.UnloadingDate
                   };
                    _context.RoutePointMappings.Add(mapping);
                }
                else
                {
                    return BadRequest("Точка маршрута не найдена");
                }
            }
            _context.SaveChanges();
        }

        return RedirectToAction("Routes");
    }

    ViewBag.AvailableRoutePoints = _context.RoutePoints.ToList();
    return View(route);
}
//вот как выглядел изначально этот метод 
 [HttpPost]
public IActionResult AddRoute(SmartLoad.Models.Rout route)
{
    if (ModelState.IsValid)
    {
        Преобразование дат в UTC
        route.DepartureDate = DateTime.SpecifyKind(route.DepartureDate, DateTimeKind.Utc);
        route.ArrivalDate = DateTime.SpecifyKind(route.ArrivalDate, DateTimeKind.Utc);

        Сохраняем временно точки маршрута
        var routePoints = route.RoutePoints.ToList();
        route.RoutePoints.Clear();

        Сначала сохраняем маршрут, чтобы получить его Id
        _context.Routes.Add(route);
        _context.SaveChanges();

        Устанавливаем RouteId для каждой точки маршрута и обновляем их
        foreach (var routePoint in routePoints)
        {
            routePoint.UnloadingDate = DateTime.SpecifyKind(routePoint.UnloadingDate, DateTimeKind.Utc);
            routePoint.RouteId = route.Id; // Устанавливаем RouteId для каждой точки разгрузки

            Проверяем, существует ли точка маршрута с заданным Id
           var existingRoutePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == routePoint.Id);
            if (existingRoutePoint != null)
            {
                Обновляем существующую точку маршрута
                existingRoutePoint.Name = routePoint.Name;
                existingRoutePoint.UnloadingDate = routePoint.UnloadingDate;
                existingRoutePoint.RouteId = routePoint.RouteId;
                _context.RoutePoints.Update(existingRoutePoint);
            }
            else
            {
                Выводим сообщение об ошибке
                return BadRequest("Точка маршрута не найдена");
            }
        }

        Сохраняем изменения для точек маршрута
        _context.SaveChanges();
        return RedirectToAction("Routes");
    }
    ViewBag.AvailableRoutePoints = _context.RoutePoints.ToList();
    return View(route);
}
*/
#endregion