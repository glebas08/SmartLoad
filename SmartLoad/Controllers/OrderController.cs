using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;
using SmartLoad.Data;

namespace SmartLoad.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Заказы

        public IActionResult Orders()
        {
            var orders = _context.Orders
                .Include(o => o.RoutePoint) // Включение данных о точках маршрута
                .Include(o => o.Distributor) // Включение данных о клиенте
                .Include(o => o.OrderProducts) // Включение данных о продуктах в заказе
                .ToList();
            return View(orders);
        }
        public IActionResult AddOrder()
        {
            // Заполняем ViewBag списком маршрутов для выбора
            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name");
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name");
            ViewBag.Products = _context.Products.ToList(); // Загрузка списка продуктов
            ViewBag.Distributors = new SelectList(_context.Distributors, "Id", "Name"); // Загрузка списка дистрибьюторов
            ViewBag.RoutePoints = new SelectList(_context.RoutePoints, "Id", "Name"); // Загрузка списка точек маршрута
            return View();
        }

        private void EnsureDateTimeKindUtc(Order order)
        {
            if (order.DeliveryDate.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"DeliveryDate must be in UTC. Current value: {order.DeliveryDate}");
            }

            if (order.RoutePoint != null && order.RoutePoint.UnloadingDate.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"UnloadingDate must be in UTC. Current value: {order.RoutePoint.UnloadingDate}");
            }

            foreach (var orderProduct in order.OrderProducts)
            {
                // Add checks for any DateTime properties in orderProduct if they exist
            }
        }

        [HttpPost]
        public IActionResult AddOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                order.DeliveryDate = DateTime.SpecifyKind(order.DeliveryDate, DateTimeKind.Utc);

                // Ensure all DateTime properties are in UTC
                EnsureDateTimeKindUtc(order);

                // Подсчитываем количество продуктов и заполняем поле ColProducts
                order.ColProducts = order.OrderProducts.Sum(op => op.Quantity);

                // Устанавливаем RoutePointId, если RoutePoint не равен null
                if (order.RoutePoint != null)
                {
                    order.RoutePointId = order.RoutePoint.Id;
                    _context.Attach(order.RoutePoint); // Привязываем существующую сущность к контексту
                }

                order.Distributor = _context.Distributors.Find(order.DistributorId);

                // Добавляем продукты в заказ
                foreach (var orderProduct in order.OrderProducts)
                {
                    orderProduct.Order = order;
                    _context.OrderProducts.Add(orderProduct);
                }

                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Orders");
            }

            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", order.RoutePointId);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", order.DistributorId);
            ViewBag.Products = _context.Products.ToList(); // Загрузка списка продуктов
            ViewBag.Distributors = new SelectList(_context.Distributors, "Id", "Name"); // Загрузка списка дистрибьюторов
            ViewBag.RoutePoints = new SelectList(_context.RoutePoints, "Id", "Name"); // Загрузка списка точек маршрута
            return View(order);
        }


        public IActionResult EditOrder(int id)
        {
            // Ищем заказ по ID
            var order = _context.Orders
                .Include(o => o.RoutePoint)
                .Include(o => o.Distributor)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Заполняем ViewBag для выпадающих списков
            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", order.RoutePoint.Id);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", order.DistributorId);

            return View(order);
        }

        [HttpPost]
        public IActionResult EditOrder(Order order)
        {
            // Проверяем валидность модели
            if (ModelState.IsValid)
            {
                try
                {
                    // Обновляем заказ в базе данных
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    return RedirectToAction("Orders");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                }
            }

            // Возвращаем данные для повторного отображения формы редактирования
            ViewData["RoutePointId"] = new SelectList(_context.RoutePoints, "Id", "Name", order.RoutePoint.Id);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", order.DistributorId);
            return View(order);
        }

        public IActionResult DeleteOrder(int id)
        {
            // Ищем заказ для подтверждения удаления
            var order = _context.Orders
                .Include(o => o.RoutePoint)
                .Include(o => o.Distributor)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("DeleteOrder")]
        public IActionResult DeleteOrderConfirmed(int id)
        {
            // Ищем заказ для удаления
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                try
                {
                    // Удаляем заказ из базы данных
                    _context.Orders.Remove(order);
                    _context.SaveChanges();

                    return RedirectToAction("Orders");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                }
            }

            return NotFound();
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
            if (ModelState.IsValid)
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

        #region Дистрибьюторы

        public IActionResult Distributors()
        {
            var distributors = _context.Distributors.ToList();
            return View(distributors);
        }

        public IActionResult AddDistributor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDistributor(Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                _context.Distributors.Add(distributor);
                _context.SaveChanges();
                return RedirectToAction("Distributors");
            }
            return View(distributor);
        }

        public IActionResult EditDistributor(int id)
        {
            var distributor = _context.Distributors.Find(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return View(distributor);
        }

        [HttpPost]
        public IActionResult EditDistributor(Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                _context.Distributors.Update(distributor);
                _context.SaveChanges();
                return RedirectToAction("Distributors");
            }
            return View(distributor);
        }

        public IActionResult DeleteDistributor(int id)
        {
            var distributor = _context.Distributors.Find(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return View(distributor);
        }

        [HttpPost, ActionName("DeleteDistributor")]
        public async Task<IActionResult> DeleteDistributorConfirmed(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            _context.Distributors.Remove(distributor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Distributors));
        }

        #endregion

        #region Точки маршрута

        public IActionResult RoutePoints(int orderId)
        {
            var routePoints = _context.RoutePoints.ToList();
            return View(routePoints);
        }

        public IActionResult AddRoutePoint()
        {
            return View(new RoutePoint());
        }

        [HttpPost]
        public IActionResult AddRoutePoint(RoutePoint routePoint)
        {
            if (ModelState.IsValid)
            {
                routePoint.UnloadingDate = DateTime.SpecifyKind(routePoint.UnloadingDate, DateTimeKind.Utc);
                _context.RoutePoints.Add(routePoint);
                _context.SaveChanges();
                return RedirectToAction("RoutePoints");
            }

            return View(routePoint);
        }

        public IActionResult EditRoutePoint(int id)
        {
            var routePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == id);

            if (routePoint == null)
            {
                return NotFound();
            }

            return View(routePoint);
        }

        [HttpPost]
        public IActionResult EditRoutePoint(RoutePoint routePoint)
        {
            if (ModelState.IsValid)
            {
                _context.RoutePoints.Update(routePoint);
                _context.SaveChanges();
                return RedirectToAction("RoutePoints");
            }

            return View(routePoint);
        }

        public IActionResult DeleteRoutePoint(int id)
        {
            var routePoint = _context.RoutePoints.FirstOrDefault(rp => rp.Id == id);

            if (routePoint == null)
            {
                return NotFound();
            }

            return View(routePoint);
        }

        [HttpPost, ActionName("DeleteRoutePoint")]
        public async Task<IActionResult> DeleteRoutePointConfirmed(int id)
        {
            var routePoint = await _context.RoutePoints.FindAsync(id);

            if (routePoint == null)
            {
                return NotFound();
            }

            _context.RoutePoints.Remove(routePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RoutePoints));
        }


        #endregion
    }
}

