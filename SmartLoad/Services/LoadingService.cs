using Microsoft.EntityFrameworkCore;
using SmartLoad.Data;
using SmartLoad.Models;

namespace SmartLoad.Services
{
    public class LoadingService
    {
        private readonly ApplicationDbContext _context;
        private double _currentTotalWeight;
        private double _currentWeightedXSum;
        private Dictionary<string, double> _currentAxleLoads;
        private const double MinXStep = 0.3;
        private const double XStepRatio = 0.5;
        private const double HeavyWeightThreshold = 300.0;
        private List<BlockPlacementStep> _placementHistory = new();

        public LoadingService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<BlockPlacementStep> GetPlacementHistory()
        {
            return _placementHistory;
        }

        public async Task<(List<CargoPlacement> Placements, string ErrorMessage)> CalculatePlacementAsync(
    Vehicle vehicle, Rout route, List<Order> orders)
        {
            // Первая попытка - стандартный алгоритм (размещение с начала)
            var firstAttempt = await TryCalculatePlacement(vehicle, route, orders);

            var (placements, errorMessage) = firstAttempt;

            // Проверяем, есть ли неразмещенные блоки
            bool allPlaced = placements.All(p => p.X >= 0 && p.Y >= 0 && p.Z >= 0);

            if (!allPlaced)
            {
                // Если есть неразмещенные блоки — проверяем, связана ли это с осями
                if (!string.IsNullOrEmpty(errorMessage) && errorMessage.Contains("задней оси тягача"))
                {
                    return (placements, "Ошибка: Перевес задней оси тягача");
                }
                else
                {
                    return (placements, "Ошибка: Не все блоки размещены");
                }
            }

            return (placements, null); // Все блоки размещены успешно
        }
        private async Task<(List<CargoPlacement> Placements, string ErrorMessage)> TryCalculatePlacement(
            Vehicle vehicle, Rout route, List<Order> orders)
        {
            InitializeState();
            var placements = new List<CargoPlacement>();
            var occupiedSpaces = new List<OccupiedSpace>();
            var placementInfo = new List<(int ProductId, float X, float Weight)>();

            try
            {
                var packagingTypes = await LoadAllPackagingTypesAsync(orders);
                if (!await ValidateLoadAsync(vehicle, orders, packagingTypes))
                    return (placements, "Превышены допустимые параметры ТС!");

                var blocks = await PrepareBlocks(vehicle, route, orders, packagingTypes);

                foreach (var block in blocks)
                {
                    if (!TryPlaceBlockAsync(vehicle, block, placements, occupiedSpaces, placementInfo))
                    {
                        placements.Add(new CargoPlacement
                        {
                            ProductId = block.ProductId,
                            X = -1,
                            Y = -1,
                            Z = -1,
                            Weight = (float)block.Weight,
                            Destination = block.Destination
                        });
                    }
                }

                var (isValid, error) = ValidateFinalLoads(vehicle, placementInfo, placements);
                if (!isValid)
                    return (placements, error);

                return (placements, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return (placements, $"Ошибка: {ex.Message}");
            }
        }

        #region Вспомогательные методы и классы
        private (bool IsValid, string ErrorMessage) ValidateFinalLoads(
     Vehicle vehicle, List<(int ProductId, float X, float Weight)> placementInfo, List<CargoPlacement> placements)
        {
            if (placements.Count == 0)
                return (true, null);

            float totalCargoWeight = 0;
            float totalMoment = 0;

            foreach (var (productId, posX, weight) in placementInfo)
            {
                totalCargoWeight += weight;
                var placement = placements.FirstOrDefault(p =>
                    p.ProductId == productId && Math.Abs(p.X - posX) < 0.001f);
                float blockLength = placement?.Length ?? 0;
                float centerX = posX + blockLength / 2;
                float distanceFromKingpin = vehicle.TrailerLength - centerX;
                totalMoment += weight * distanceFromKingpin;
            }

            float cargoPositionFromKingpin = totalCargoWeight > 0
                ? totalMoment / totalCargoWeight
                : 0;

            var finalLoads = vehicle.CalculateAxleLoads(totalCargoWeight, cargoPositionFromKingpin);

            var errorDetails = new List<string>();

            if (finalLoads.TryGetValue("TractorFrontAxle", out float frontAxleLoad) &&
                frontAxleLoad > vehicle.TractorMaxFrontAxleLoad)
            {
                errorDetails.Add($"Перегруз передней оси: {frontAxleLoad:N0} > {vehicle.TractorMaxFrontAxleLoad:N0} кг");
            }

            if (finalLoads.TryGetValue("TractorRearAxle", out float rearAxleLoad) &&
                rearAxleLoad > vehicle.TractorMaxRearAxleLoad)
            {
                errorDetails.Add($"Перевес задней оси тягача: {rearAxleLoad:N0} > {vehicle.TractorMaxRearAxleLoad:N0} кг");
            }

            if (finalLoads.TryGetValue("TrailerAxles", out float trailerAxleLoad) &&
                trailerAxleLoad > (vehicle.TrailerMaxAxleLoad * vehicle.TrailerAxleCount))
            {
                errorDetails.Add($"Перегруз осей полуприцепа: {trailerAxleLoad:N0} > {vehicle.TrailerMaxAxleLoad * vehicle.TrailerAxleCount:N0} кг");
            }

            if (errorDetails.Count > 0)
            {
                return (false, "Финальная проверка: " + string.Join("; ", errorDetails));
            }

            return (true, null);
        }
        private void InitializeState()
        {
            _currentTotalWeight = 0;
            _currentWeightedXSum = 0;
            _currentAxleLoads = new Dictionary<string, double>
            {
                ["TractorFrontAxle"] = 0,
                ["TractorRearAxle"] = 0,
                ["TrailerAxles"] = 0
            };
        }
        private async Task<List<Block>> PrepareBlocks(Vehicle vehicle, Rout route, List<Order> orders, Dictionary<int, PackagingType> packagingTypes)
        {
            var allBlocks = new List<Block>();

            var routePoints = route.RoutePointMappings
                .OrderBy(rpm => rpm.OrderInRoute)
                .Select(rpm => rpm.RoutePoint)
                .ToList();

            for (int i = 0; i < routePoints.Count; i++)
            {
                var ordersForPoint = orders.Where(o => o.RoutePointId == routePoints[i].Id).ToList();
                var blocks = FormBlocks(ordersForPoint, packagingTypes, routePoints[i].Name, i + 1);
                allBlocks.AddRange(blocks);
            }

            return allBlocks
                .OrderByDescending(b => b.RoutePointPriority)
                .ThenByDescending(b => b.Weight)
                .ToList();
        }
        private bool TryPlaceBlockAsync(Vehicle vehicle, Block block, List<CargoPlacement> placements,
        List<OccupiedSpace> occupiedSpaces, List<(int ProductId, float X, float Weight)> placementInfo)
        {
            if (block.Length <= 0 || block.Width <= 0 || block.Height <= 0 || block.Weight <= 0)
            {
                throw new ArgumentException($"Некорректные размеры или вес блока: ProductId={block.ProductId}");
            }
            var orientations = GetPossibleOrientations(block);
            double trailerLength = vehicle.TrailerLength;
            double trailerWidth = vehicle.TrailerWidth;
            double trailerHeight = vehicle.TrailerHeight;
            foreach (var orientation in orientations)
            {
                double xStep = Math.Max(orientation.Length * XStepRatio, MinXStep);

                for (double x = trailerLength - orientation.Length; x >= 0; x -= xStep)
                {
                    if (block.Weight > HeavyWeightThreshold && x > trailerLength * 0.7)
                        continue;
                    Dictionary<double, double> yLevels = new Dictionary<double, double>();
                    double centerY = (trailerWidth - orientation.Width) / 2;
                    double maxYOffset = Math.Max(centerY, trailerWidth - centerY - orientation.Width);

                    List<double> yPositions = new List<double>();
                    for (double yOffset = 0; yOffset <= maxYOffset; yOffset += orientation.Width)
                    {
                        double yRight = centerY + yOffset;
                        if (yRight + orientation.Width <= trailerWidth)
                        {
                            yPositions.Add(yRight);
                        }

                        if (yOffset > 0)
                        {
                            double yLeft = centerY - yOffset;
                            if (yLeft >= 0)
                            {
                                yPositions.Add(yLeft);
                            }
                        }
                    }

                    foreach (var y in yPositions)
                    {
                        double currentHeight = 0;
                        var supportingBlocks = occupiedSpaces
                            .Where(os =>
                                x < os.X + os.Length &&
                                x + orientation.Length > os.X &&
                                y < os.Y + os.Width &&
                                y + orientation.Width > os.Y)
                            .OrderByDescending(os => os.Z + os.Height)
                            .FirstOrDefault();

                        if (supportingBlocks != null)
                        {
                            currentHeight = supportingBlocks.Z + supportingBlocks.Height;
                        }
                        yLevels[y] = currentHeight;
                    }

                    if (yLevels.Count > 0)
                    {
                        var bestY = yLevels.OrderBy(kvp => kvp.Value).First().Key;
                        double z = yLevels[bestY];
                        if (z + orientation.Height <= trailerHeight)
                        {
                            if (CanPlaceBlock(occupiedSpaces, x, bestY, z, orientation, block.RoutePointPriority) &&
                                CheckAxleLoads(vehicle, block, x, orientation.Length, placementInfo, placements))
                            {
                                PlaceBlock(vehicle, block, x, bestY, z, orientation, placements, occupiedSpaces, placementInfo);
                                _placementHistory.Add(new BlockPlacementStep
                                {
                                    StepNumber = _placementHistory.Count + 1,
                                    ProductId = block.ProductId,
                                    ProductName = GetProductName(block.ProductId), // Реализуйте этот метод или замените на реальное имя
                                    PositionX = x,
                                    PositionY = bestY,
                                    PositionZ = z,
                                    Length = orientation.Length,
                                    Width = orientation.Width,
                                    Height = orientation.Height,
                                    Weight = block.Weight,
                                    Destination = block.Destination
                                });
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private string GetProductName(int productId)
        {
            // Замените на реальный запрос к БД, если нужно
            return $"Товар #{productId}";
        }
        private List<Orientation> GetPossibleOrientations(Block block)
        {
            return new List<Orientation>
            {
                new Orientation(block.Length, block.Width, block.Height),
                new Orientation(block.Width, block.Length, block.Height)
            };
        }
        private bool CanPlaceBlock(List<OccupiedSpace> occupiedSpaces, double x, double y, double z,
            Orientation orientation, int newBlockPriority)
        {
            // Находим ВСЕ блоки под текущей позицией (не только точно снизу)
            var supportingBlocks = occupiedSpaces
                .Where(os =>
                    x < os.X + os.Length &&
                    x + orientation.Length > os.X &&
                    y < os.Y + os.Width &&
                    y + orientation.Width > os.Y &&
                    z >= os.Z && z <= os.Z + os.Height) // Любая поддержка по Z
                .ToList();

            // Проверяем приоритеты опорных блоков
            foreach (var supportingBlock in supportingBlocks)
            {
                if (supportingBlock.RoutePointPriority < newBlockPriority)
                    return false;
            }

            // Проверка коллизий с другими блоками
            return !occupiedSpaces.Any(os =>
                x < os.X + os.Length &&
                x + orientation.Length > os.X &&
                y < os.Y + os.Width &&
                y + orientation.Width > os.Y &&
                z < os.Z + os.Height &&
                z + orientation.Height > os.Z);
        }
        private bool CheckAxleLoads(Vehicle vehicle, Block block, double x, double length,
         List<(int ProductId, float X, float Weight)> placementInfo, List<CargoPlacement> placements)
        {
            // Создаем временный список с уже размещенными блоками
            var tempPlacementInfo = new List<(int ProductId, float X, float Weight)>(placementInfo);

            // Добавляем новый блок
            tempPlacementInfo.Add((block.ProductId, (float)x, (float)block.Weight));

            float totalCargoWeight = 0;
            float totalMoment = 0;

            foreach (var (productId, posX, weight) in tempPlacementInfo)
            {
                totalCargoWeight += weight;

                // Определяем длину блока
                float blockLength;
                if (productId == block.ProductId && Math.Abs(posX - (float)x) < 0.001f)
                {
                    // Это новый блок, используем его длину
                    blockLength = (float)length;
                }
                else
                {
                    // Это существующий блок, ищем его в placements
                    var placement = placements.FirstOrDefault(p => p.ProductId == productId && Math.Abs(p.X - posX) < 0.001f);
                    blockLength = placement?.Length ?? 0;
                }

                // Рассчитываем центр блока и расстояние от шкворня
                float centerX = posX + blockLength / 2;
                float distanceFromKingpin = vehicle.TrailerLength - centerX;

                // Рассчитываем момент
                totalMoment += weight * distanceFromKingpin;
            }

            float cargoPositionFromKingpin = 0;
            if (totalCargoWeight > 0)
            {
                cargoPositionFromKingpin = totalMoment / totalCargoWeight;
            }

            // Используем метод CalculateAxleLoads для расчета нагрузок
            var newLoads = vehicle.CalculateAxleLoads(totalCargoWeight, cargoPositionFromKingpin);

            // Для отладки
            Console.WriteLine($"CheckAxleLoads: Вес={totalCargoWeight}, Расстояние={cargoPositionFromKingpin}");
            // Используем метод ValidateAxleLoads для проверки допустимости нагрузок
            return vehicle.ValidateAxleLoads(newLoads);
        }
        private void PlaceBlock(Vehicle vehicle, Block block, double x, double y, double z, Orientation orientation,
                    List<CargoPlacement> placements, List<OccupiedSpace> occupiedSpaces, List<(int ProductId, float X, float Weight)> placementInfo)
        {
            // Добавляем информацию о размещении для расчета нагрузок
            // Важно: сохраняем только координату X, без учета центра блока
            placementInfo.Add((block.ProductId, (float)x, (float)block.Weight));


            // Обновляем общий вес и взвешенную сумму
            _currentTotalWeight += block.Weight;
            _currentWeightedXSum += block.Weight * (x + orientation.Length / 2);

            // Добавляем информацию о занятом пространстве
            occupiedSpaces.Add(new OccupiedSpace
            {
                X = x,
                Y = y,
                Z = z,
                Length = orientation.Length,
                Width = orientation.Width,
                Height = orientation.Height,
                RoutePointPriority = block.RoutePointPriority
            });

            // Создаем объект размещения груза
            placements.Add(new CargoPlacement
            {
                ProductId = block.ProductId,
                X = (float)x,
                Y = (float)y,
                Z = (float)z,
                Length = (float)orientation.Length,
                Width = (float)orientation.Width,
                Height = (float)orientation.Height,
                Weight = (float)block.Weight,
                Destination = block.Destination
            });
        }
        private async Task<Dictionary<int, PackagingType>> LoadAllPackagingTypesAsync(List<Order> orders)
        {
            var productIds = orders
                .SelectMany(o => o.OrderProducts.Select(op => op.ProductId))
                .Distinct()
                .ToList();

            return await _context.PackagingTypes
                .Where(pt => productIds.Contains(pt.ProductId))
                .ToDictionaryAsync(pt => pt.ProductId);
        }

        private List<Block> FormBlocks(List<Order> orders, Dictionary<int, PackagingType> packagingTypes, string destination, int routePointPriority)
        {
            var blocks = new List<Block>();

            foreach (var order in orders)
            {
                foreach (var orderProduct in order.OrderProducts)
                {
                    if (!packagingTypes.TryGetValue(orderProduct.ProductId, out var packagingType))
                    {
                        throw new InvalidOperationException($"Не найден тип упаковки для продукта с ID: {orderProduct.ProductId}");
                    }

                    if (packagingType.Length <= 0 || packagingType.Width <= 0 ||
                        packagingType.Height <= 0 || packagingType.Weight <= 0)
                    {
                        throw new InvalidOperationException(
                            $"Некорректные данные упаковки для продукта с ID: {orderProduct.ProductId}");
                    }

                    for (int i = 0; i < orderProduct.Quantity; i++)
                    {
                        blocks.Add(new Block
                        {
                            ProductId = orderProduct.ProductId,
                            Length = packagingType.Length,
                            Width = packagingType.Width,
                            Height = packagingType.Height,
                            Weight = packagingType.Weight,
                            Destination = destination,
                            RoutePointPriority = routePointPriority
                        });
                    }
                }
            }

            return blocks;
        }

        private async Task<bool> ValidateLoadAsync(Vehicle vehicle, List<Order> orders, Dictionary<int, PackagingType> packagingTypes)
        {
            double totalWeight = 0;
            double totalVolume = 0;

            foreach (var order in orders)
            {
                foreach (var orderProduct in order.OrderProducts)
                {
                    if (!packagingTypes.TryGetValue(orderProduct.ProductId, out var packagingType))
                    {
                        throw new InvalidOperationException($"Не найден тип упаковки для продукта с ID: {orderProduct.ProductId}");
                    }

                    totalWeight += orderProduct.Quantity * packagingType.Weight;
                    totalVolume += orderProduct.Quantity *
                        (packagingType.Length * packagingType.Width * packagingType.Height) / 1_000_000_000;
                }
            }

            double vehicleVolume = (vehicle.TrailerLength * vehicle.TrailerWidth * vehicle.TrailerHeight) / 1_000_000_000;
            return totalWeight <= vehicle.TrailerMaxLoadCapacity && totalVolume <= vehicleVolume;
        }

        private class Block
        {
            public int ProductId { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public double Weight { get; set; }
            public string Destination { get; set; }
            public int RoutePointPriority { get; set; }
        }

        private class OccupiedSpace
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public int RoutePointPriority { get; set; }
        }
        private record Orientation(double Length, double Width, double Height);
        #endregion
    }
}