using System;
using System.Collections.Generic;
using System.Linq;
using SmartLoad.Models;

namespace SmartLoad.Services
{
    public class LoadingService
    {
        // Основной метод для расчёта размещения груза
        //public List<CargoPlacement> CalculatePlacement(VehicleType vehicle, Rout route, List<Order> orders)
        //{
        //    var placements = new List<CargoPlacement>(); // Список для хранения координат размещения продуктов

        //    // Проверка совместимости грузов с транспортом
        //    if (!ValidateLoad(vehicle, orders))
        //    {
        //        throw new InvalidOperationException("Груз превышает допустимые параметры транспортного средства!");
        //    }

        //    // Координаты начала размещения
        //    double currentX = 0;
        //    double currentY = 0;
        //    double currentZ = 0;

        //    // Проходимся по точкам маршрута
        //    foreach (var routePoint in route.RoutePoints)
        //    {
        //        // Получаем заказы для текущей точки
        //        var ordersForPoint = orders.Where(o => o.RoutePointId == routePoint.Id).ToList();

        //        foreach (var order in ordersForPoint)
        //        {
        //            foreach (var orderProduct in order.OrderProducts)
        //            {
        //                for (int i = 0; i < orderProduct.Quantity; i++) // Размещаем каждый экземпляр продукта
        //                {
        //                    // Проверка на выход за пределы транспортного средства
        //                    if (currentX + orderProduct.Product.PackagingTypes .Length > vehicle.Length)
        //                    {
        //                        currentX = 0;
        //                        currentY += orderProduct.Product.Width;

        //                        if (currentY > vehicle.Width)
        //                        {
        //                            currentY = 0;
        //                            currentZ += orderProduct.Product.Height;
        //                        }
        //                    }

        //                    if (currentZ + orderProduct.Product.Height > vehicle.Height)
        //                    {
        //                        throw new InvalidOperationException("Невозможно разместить груз в транспортном средстве!");
        //                    }

        //                    // Добавляем координаты для продукта
        //                    placements.Add(new CargoPlacement
        //                    {
        //                        ProductId = orderProduct.Product.Id,
        //                        X = currentX,
        //                        Y = currentY,
        //                        Z = currentZ,
        //                        Destination = routePoint.Name // Точка выгрузки
        //                    });

        //                    // Сдвигаем координаты для следующего продукта
        //                    currentX += orderProduct.Product.Length;
        //                }
        //            }
        //        }
        //    }

        //    return placements; // Возвращаем список координат размещения
        //}

        //// Метод для проверки, помещается ли груз в транспортное средство
        //private bool ValidateLoad(VehicleType vehicle, List<Order> orders)
        //{
        //    double totalWeight = orders.Sum(o => o.OrderProducts.Sum(p => p.Quantity * p.Product.Weight));
        //    double totalVolume = orders.Sum(o => o.OrderProducts.Sum(p =>
        //        p.Quantity * (p.Product.PackagingTypes.Length * p.Product.Width * p.Product.Height)));

        //    return totalWeight <= vehicle.LoadCapacity && totalVolume <= GetVehicleVolume(vehicle);
        //}

        //// Метод для расчёта объёма транспортного средства
        //private double GetVehicleVolume(VehicleType vehicle)
        //{
        //    return vehicle.Length * vehicle.Width * vehicle.Height;
        //}
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using SmartLoad.Data;
//using SmartLoad.Models;

////Сеервис который будет управлять логикой создания и обновления схем погрузки.

//namespace SmartLoad.Services
//{
//    public class LoadingService
//    {
//        private readonly ApplicationDbContext _context;

//        public LoadingService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // Метод для создания схемы погрузки
//        public async Task<LoadingScheme> CreateLoadingScheme(int vehicleId, int routeId)
//        {
//            var vehicle = await _context.Vehicles
//                .Include(v => v.VehicleType)
//                .FirstOrDefaultAsync(v => v.Id == vehicleId);

//            if (vehicle == null)
//            {
//                throw new ArgumentException("Транспортное средство не найдено.");
//            }

//            var route = await _context.Routes
//                .Include(r => r.RoutePoints)
//                .ThenInclude(rp => rp.OrderRoutePoints)
//                .ThenInclude(orp => orp.Order)
//                .ThenInclude(o => o.OrderProducts)
//                .ThenInclude(op => op.Product)
//                .ThenInclude(p => p.PackagingTypes)
//                .FirstOrDefaultAsync(r => r.Id == routeId);

//            if (route == null)
//            {
//                throw new ArgumentException("Маршрут не найден.");
//            }

//            var loadingScheme = new LoadingScheme
//            {
//                VehicleTypeId = vehicle.VehicleTypeId,
//                VehicleId = vehicleId,
//                RouteId = routeId,
//                LoadingDate = route.DepartureDate,
//                Status = "Создана",
//                Notes = $"Схема загрузки для маршрута {route.Name}",
//                LoadingProducts = new List<LoadingProduct>()
//            };

//            _context.LoadingSchemes.Add(loadingScheme);
//            await _context.SaveChangesAsync();

//            // Сортируем точки маршрута по дате выгрузки
//            var sortedRoutePoints = route.RoutePoints.OrderBy(rp => rp.UnloadingDate).ToList();

//            float currentX = 0;
//            float currentY = 0;
//            float currentZ = 0;

//            float totalWeight = 0;
//            float totalVolume = 0;

//            foreach (var routePoint in sortedRoutePoints)
//            {
//                foreach (var orp in routePoint.OrderRoutePoints)
//                {
//                    var order = orp.Order;
//                    foreach (var orderProduct in order.OrderProducts)
//                    {
//                        var product = await _context.Products
//                            .Include(p => p.PackagingTypes)
//                            .FirstOrDefaultAsync(p => p.Id == orderProduct.ProductId);

//                        if (product == null)
//                        {
//                            throw new ArgumentException("Продукт не найден.");
//                        }

//                        var packagingType = product.PackagingTypes.FirstOrDefault();
//                        if (packagingType == null)
//                        {
//                            throw new ArgumentException("Тип упаковки не найден для продукта.");
//                        }

//                        for (int i = 0; i < orderProduct.Quantity; i++)
//                        {
//                            var loadingProduct = new LoadingProduct
//                            {
//                                LoadingSchemeId = loadingScheme.Id,
//                                ProductId = orderProduct.ProductId,
//                                OrderId = orderProduct.OrderId,
//                                RoutePointId = routePoint.Id,
//                                Quantity = 1,
//                                PositionX = currentX,
//                                PositionY = currentY,
//                                PositionZ = currentZ
//                            };

//                            loadingScheme.LoadingProducts.Add(loadingProduct);

//                            currentX += packagingType.Length;
//                            if (currentX > vehicle.VehicleType.Length)
//                            {
//                                currentX = 0;
//                                currentY += packagingType.Width;
//                            }
//                            if (currentY > vehicle.VehicleType.Width)
//                            {
//                                currentY = 0;
//                                currentZ += packagingType.Height;
//                            }

//                            totalWeight += packagingType.Weight;
//                            totalVolume += packagingType.Volume;

//                            if (totalWeight > vehicle.MaxLoadCapacityTractor + vehicle.MaxLoadCapacityTrailer)
//                            {
//                                throw new Exception("Превышена максимальная грузоподъемность.");
//                            }

//                            if (totalVolume > vehicle.MaxVolumeCapacityTractor + vehicle.MaxVolumeCapacityTrailer)
//                            {
//                                throw new Exception("Превышен максимальный объем.");
//                            }
//                        }
//                    }
//                }
//            }

//            // Расчет позиций продуктов и проверка условий безопасности
//            CalculateProductPositions(loadingScheme);

//            _context.LoadingSchemes.Update(loadingScheme);
//            await _context.SaveChangesAsync();
//            return loadingScheme;
//        }

//        private void CalculateProductPositions(LoadingScheme loadingScheme)
//        {
//            var vehicle = _context.Vehicles
//                .Include(v => v.VehicleType)
//                .FirstOrDefault(v => v.Id == loadingScheme.VehicleId);

//            if (vehicle == null)
//            {
//                throw new ArgumentException("Транспортное средство не найдено.");
//            }

//            var loadingProducts = loadingScheme.LoadingProducts
//                .OrderBy(lp => lp.RoutePoint.UnloadingDate)
//                .ToList();

//            float currentX = 0;
//            float currentY = 0;
//            float currentZ = 0;

//            float totalWeight = 0;
//            float totalVolume = 0;

//            foreach (var loadingProduct in loadingProducts)
//            {
//                var product = _context.Products
//                    .Include(p => p.PackagingTypes)
//                    .FirstOrDefault(p => p.Id == loadingProduct.ProductId);

//                if (product == null)
//                {
//                    throw new ArgumentException("Продукт не найден.");
//                }

//                var packagingType = product.PackagingTypes.FirstOrDefault();
//                if (packagingType == null)
//                {
//                    throw new ArgumentException("Тип упаковки не найден для продукта.");
//                }

//                loadingProduct.PositionX = currentX;
//                loadingProduct.PositionY = currentY;
//                loadingProduct.PositionZ = currentZ;

//                currentX += packagingType.Length;
//                if (currentX > vehicle.VehicleType.Length)
//                {
//                    currentX = 0;
//                    currentY += packagingType.Width;
//                }
//                if (currentY > vehicle.VehicleType.Width)
//                {
//                    currentY = 0;
//                    currentZ += packagingType.Height;
//                }

//                totalWeight += packagingType.Weight * loadingProduct.Quantity;
//                totalVolume += packagingType.Volume * loadingProduct.Quantity;

//                if (totalWeight > vehicle.MaxLoadCapacityTractor + vehicle.MaxLoadCapacityTrailer)
//                {
//                    throw new Exception("Превышена максимальная грузоподъемность.");
//                }

//                if (totalVolume > vehicle.MaxVolumeCapacityTractor + vehicle.MaxVolumeCapacityTrailer)
//                {
//                    throw new Exception("Превышен максимальный объем.");
//                }

//                _context.LoadingProducts.Update(loadingProduct);
//            }

//            _context.SaveChanges();
//        }
//    }
//}