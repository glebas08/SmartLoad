using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Data;
using SmartLoad.Models;
using SmartLoad.Services;
using SmartLoad.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SmartLoad.Controllers
{
    public class LoadingSchemeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LoadingService _loadingService;
        //private readonly IMemoryCache _cache;
        private readonly ILogger<LoadingSchemeController> _logger;

        public LoadingSchemeController(ApplicationDbContext context, LoadingService loadingService, IMemoryCache cache, ILogger<LoadingSchemeController> logger)
        {
            _context = context;
            _loadingService = loadingService;
            //_cache = cache;
            _logger = logger;
        }

        // GET: LoadingScheme
        public async Task<IActionResult> Index()
        {
            var loadingSchemes = await _context.LoadingSchemes
                .Include(ls => ls.Vehicle)
                .Include(ls => ls.Rout)
                .ToListAsync();
            return View(loadingSchemes);
        }

        // GET: LoadingScheme/Create
        public async Task<IActionResult> Create()
        {
            // Получаем список доступных транспортных средств и маршрутов для выпадающих списков
            ViewBag.Vehicles = new SelectList(await _context.Vehicles.ToListAsync(), "Id", "Name");
            ViewBag.Routes = new SelectList(await _context.Routes.ToListAsync(), "Id", "Name");

            return View();
        }

        // POST: LoadingScheme/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int vehicleId, int routeId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var vehicle = await _context.Vehicles.FindAsync(vehicleId);
                    if (vehicle == null)
                    {
                        return NotFound("Транспортное средство не найдено");
                    }

                    var route = await _context.Routes
                        .Include(r => r.RoutePointMappings)
                            .ThenInclude(rpm => rpm.RoutePoint)
                        .FirstOrDefaultAsync(r => r.Id == routeId);

                    if (route == null)
                    {
                        return NotFound("Маршрут не найден");
                    }

                    var routePointIds = route.RoutePointMappings.Select(rpm => rpm.RoutePoint.Id).ToList();
                    var orders = await _context.Orders
                        .Include(o => o.OrderProducts)
                            .ThenInclude(op => op.Product)
                        .Where(o => o.RoutePointId.HasValue && routePointIds.Contains(o.RoutePointId.Value))
                        .ToListAsync();

                    var loadingScheme = new LoadingScheme
                    {
                        VehicleId = vehicleId,
                        RoutId = routeId,
                        CreatedDate = DateTime.UtcNow,
                        LoadingDate = DateTime.UtcNow,
                        Status = "Создана"
                    };

                    _context.LoadingSchemes.Add(loadingScheme);
                    await _context.SaveChangesAsync();

                    try
                    {
                        // Используем CalculatePlacementAsync напрямую
                        var (placements, errorMessage) = await _loadingService.CalculatePlacementAsync(vehicle, route, orders);

                        // Устанавливаем статус схемы в зависимости от результата
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            loadingScheme.Status = $"Ошибка: {errorMessage}";
                        }
                        else
                        {
                            loadingScheme.Status = "Создана";

                            // Сохраняем размещения только если нет ошибок
                            foreach (var placement in placements)
                            {
                                var loadingItem = new LoadingSchemeItem
                                {
                                    LoadingSchemeId = loadingScheme.Id,
                                    ProductId = placement.ProductId,
                                    PositionX = (float)placement.X,
                                    PositionY = (float)placement.Y,
                                    PositionZ = (float)placement.Z,
                                    Destination = placement.Destination
                                };
                                _context.LoadingSchemeItems.Add(loadingItem);
                            }
                        }
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Details), new { id = loadingScheme.Id });
                    }
                    catch (Exception ex)
                    {
                        _context.LoadingSchemes.Remove(loadingScheme);
                        await _context.SaveChangesAsync();
                        TempData["ErrorMessage"] = ex.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка при создании схемы загрузки: {ex.Message}");
                }
            }

            ViewBag.Vehicles = new SelectList(await _context.Vehicles.ToListAsync(), "Id", "Name", vehicleId);
            ViewBag.Routes = new SelectList(await _context.Routes.ToListAsync(), "Id", "Name", routeId);
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult SaveViewPreference([FromBody] ViewPreferenceModel model)
        {
            try
            {
                // Сохраняем предпочтение пользователя в сессии или куки
                HttpContext.Session.SetString($"ViewType_{model.SchemeId}", model.ViewType);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Модифицируйте метод Details для использования кэшированных данных
        // Предположим, что у вас есть метод Details, который нужно изменить
        // Модифицированный метод Details для использования LoadingSchemeViewModel
        public async Task<IActionResult> Details(int id)
        {
            var loadingScheme = await _context.LoadingSchemes
                .Include(ls => ls.Vehicle)
                .Include(ls => ls.Rout)
                .ThenInclude(r => r.RoutePointMappings)
                .ThenInclude(rpm => rpm.RoutePoint)
                .FirstOrDefaultAsync(ls => ls.Id == id);

            if (loadingScheme == null)
            {
                return NotFound();
            }

            // Получаем заказы для маршрута
            var routePointIds = loadingScheme.Rout.RoutePointMappings
                .Select(rpm => rpm.RoutePointId)
                .ToList();

            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ThenInclude(p => p.PackagingTypes)
                .Where(o => routePointIds.Contains(o.RoutePointId.Value))
                .ToListAsync();

            // Вызываем метод CalculatePlacementAsync напрямую
            var (placements, errorMessage) = await _loadingService.CalculatePlacementAsync(
                loadingScheme.Vehicle,
                loadingScheme.Rout,
                orders);

            // Обновляем статус схемы загрузки, если есть ошибка
            if (!string.IsNullOrEmpty(errorMessage))
            {
                loadingScheme.Status = $"Ошибка: {errorMessage}";
                await _context.SaveChangesAsync();
            }
            else if (loadingScheme.Status.StartsWith("Ошибка:") || loadingScheme.Status == "Создана")
            {
                // Если ранее была ошибка, но теперь всё в порядке, обновляем статус
                loadingScheme.Status = "Создана";
                await _context.SaveChangesAsync();
            }

            // Получаем информацию о продуктах для отображения
            var products = await _context.Products
                .Include(p => p.PackagingTypes)
                .Where(p => placements.Select(pl => pl.ProductId).Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            // Группируем размещения по пунктам назначения
            var destinationGroups = new List<DestinationGroupViewModel>();
            var routePoints = loadingScheme.Rout.RoutePointMappings
                .OrderBy(rpm => rpm.OrderInRoute)
                .Select(rpm => rpm.RoutePoint)
                .ToList();

            for (int i = 0; i < routePoints.Count; i++)
            {
                var routePoint = routePoints[i];
                var placementsForPoint = placements
                    .Where(p => p.Destination == routePoint.Name)
                    .ToList();

                var items = new List<LoadingProductViewModel>();
                foreach (var placement in placementsForPoint)
                {
                    if (products.TryGetValue(placement.ProductId, out var product))
                    {
                        var item = new LoadingProductViewModel
                        {
                            Id = placement.ProductId,
                            ProductName = product.Name,
                            Length = product.PackagingTypes.FirstOrDefault()?.Length ?? 0,
                            Width = product.PackagingTypes.FirstOrDefault()?.Width ?? 0,
                            Height = product.PackagingTypes.FirstOrDefault()?.Height ?? 0,
                            PositionX = (float)placement.X,
                            PositionY = (float)placement.Y,
                            PositionZ = (float)placement.Z,
                            Destination = placement.Destination,
                            Product = product
                        };

                        items.Add(item);
                    }
                }

                destinationGroups.Add(new DestinationGroupViewModel
                {
                    Destination = routePoint.Name,
                    ColorIndex = i,
                    Items = items
                });
            }

            // Рассчитываем общий вес груза и его положение
            float totalCargoWeight = 0;
            float totalMoment = 0;

            foreach (var placement in placements.Where(p => p.X >= 0 && p.Y >= 0 && p.Z >= 0)) // Учитываем только размещенные блоки
            {
                var product = products.GetValueOrDefault(placement.ProductId);
                float itemWeight = product?.PackagingTypes?.FirstOrDefault()?.Weight ?? 0;

                totalCargoWeight += itemWeight;

                // Рассчитываем момент (вес * расстояние от шкворня)
                // Используем центр блока для расчета момента
                float centerX = (float)placement.X + (placement.Length / 2);
                float distanceFromKingpin = loadingScheme.Vehicle.TrailerLength - centerX;
                totalMoment += itemWeight * distanceFromKingpin;
            }

            // Рассчитываем среднее расстояние от шкворня до центра тяжести
            float cargoPositionFromKingpin = 0;
            if (totalCargoWeight > 0)
            {
                cargoPositionFromKingpin = totalMoment / totalCargoWeight;
            }

            // Используем метод Vehicle для расчета нагрузок на оси
            var axleLoads = loadingScheme.Vehicle.CalculateAxleLoads(totalCargoWeight, cargoPositionFromKingpin);

            // Создаем модель представления
            var viewModel = new LoadingSchemeViewModel
            {
                LoadingScheme = loadingScheme,
                DestinationGroups = destinationGroups,
                AxleLoads = axleLoads,
                ErrorMessage = errorMessage,
                ViewType = HttpContext.Session.GetString($"ViewType_{id}") ?? "3D"
            };

            return View(viewModel);
        }



        // GET: LoadingScheme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loadingScheme = await _context.LoadingSchemes
                .Include(ls => ls.Vehicle)
                .Include(ls => ls.Rout)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (loadingScheme == null)
            {
                return NotFound();
            }

            return View(loadingScheme);
        }

        // POST: LoadingScheme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loadingScheme = await _context.LoadingSchemes.FindAsync(id);
            if (loadingScheme == null)
            {
                return NotFound();
            }

            // Удаляем все связанные элементы схемы загрузки
            var loadingSchemeItems = await _context.LoadingSchemeItems
                .Where(lsi => lsi.LoadingSchemeId == id)
                .ToListAsync();
            _context.LoadingSchemeItems.RemoveRange(loadingSchemeItems);

            // Удаляем схему загрузки
            _context.LoadingSchemes.Remove(loadingScheme);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: LoadingScheme/DeleteProduct/5
        public async Task<IActionResult> DeleteProduct(int? id, int? loadingSchemeId)
        {
            if (id == null || loadingSchemeId == null)
            {
                return NotFound();
            }

            var loadingSchemeItem = await _context.LoadingSchemeItems
                .Include(lsi => lsi.Product)
                .Include(lsi => lsi.LoadingScheme)
                .FirstOrDefaultAsync(m => m.Id == id && m.LoadingSchemeId == loadingSchemeId);

            if (loadingSchemeItem == null)
            {
                return NotFound();
            }

            return View(loadingSchemeItem);
        }

        // POST: LoadingScheme/DeleteProduct/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id, int loadingSchemeId)
        {
            var loadingSchemeItem = await _context.LoadingSchemeItems
                .FirstOrDefaultAsync(lsi => lsi.Id == id && lsi.LoadingSchemeId == loadingSchemeId);

            if (loadingSchemeItem == null)
            {
                return NotFound();
            }

            _context.LoadingSchemeItems.Remove(loadingSchemeItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = loadingSchemeId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCoordinates()
        {
            try
            {
                // Получаем данные из запроса
                var jsonData = Request.Form.Files["jsonData"];
                var filename = Request.Form["filename"];

                if (jsonData == null || string.IsNullOrEmpty(filename))
                {
                    return Json(new { success = false, message = "Не указаны данные или имя файла" });
                }

                // Создаем директорию для сохранения файлов, если она не существует
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coordinates");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Формируем путь к файлу
                var filePath = Path.Combine(uploadsFolder, filename);

                // Сохраняем файл
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await jsonData.CopyToAsync(fileStream);
                }

                return Json(new { success = true, filePath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetSavedCoordinates(int schemeId)
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coordinates");
                if (!Directory.Exists(uploadsFolder))
                {
                    return Json(new { files = new List<object>() });
                }

                var files = Directory.GetFiles(uploadsFolder)
                    .Where(f => Path.GetFileName(f).Contains($"_{schemeId}.json"))
                    .Select(f => new {
                        name = Path.GetFileName(f),
                        path = $"/coordinates/{Path.GetFileName(f)}",
                        date = System.IO.File.GetCreationTime(f)
                    })
                    .OrderByDescending(f => f.date)
                    .ToList();

                return Json(new { files });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, files = new List<object>() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveCoordinatesAsText([FromBody] SaveCoordinatesRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Data))
                {
                    return Json(new { success = false, message = "Не указаны данные для сохранения" });
                }

                // Создаем директорию для сохранения файлов, если она не существует
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coordinates");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filename;
                string filePath;

                if (request.Format.ToLower() == "docx")
                {
                    // Сохранение в DOCX формате
                    filename = $"{request.Type}_{request.SchemeId}_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                    filePath = Path.Combine(uploadsFolder, filename);

                    using (var document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                    {
                        // Добавляем основные части документа
                        var mainPart = document.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        var body = mainPart.Document.AppendChild(new Body());

                        // Добавляем заголовок
                        var title = body.AppendChild(new Paragraph());
                        var titleRun = title.AppendChild(new Run());
                        titleRun.AppendChild(new Text($"Координаты {(request.Type == "cargo" ? "груза" : "полуприцепа")} для схемы загрузки #{request.SchemeId}"));

                        // Добавляем дату
                        var date = body.AppendChild(new Paragraph());
                        var dateRun = date.AppendChild(new Run());
                        dateRun.AppendChild(new Text($"Дата: {DateTime.Now:dd.MM.yyyy HH:mm:ss}"));

                        // Добавляем пустую строку
                        body.AppendChild(new Paragraph());

                        // Добавляем данные
                        var data = body.AppendChild(new Paragraph());
                        var dataRun = data.AppendChild(new Run());
                        dataRun.AppendChild(new Text(request.Data));

                        mainPart.Document.Save();
                    }
                }
                else
                {
                    // Сохранение в TXT формате
                    filename = $"{request.Type}_{request.SchemeId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    filePath = Path.Combine(uploadsFolder, filename);
                    await System.IO.File.WriteAllTextAsync(filePath, request.Data);
                }

                return Json(new { success = true, filePath = $"/coordinates/{filename}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Класс для десериализации запроса
        public class SaveCoordinatesRequest
        {
            public int SchemeId { get; set; }
            public string Type { get; set; } // "cargo" или "trailer"
            public string Data { get; set; } // Текстовые данные для сохранения
            public string Format { get; set; } // "txt" или "docx"
        }

        // POST: LoadingScheme/ChangeView
        [HttpPost]
        public IActionResult ChangeView(int id, string viewType)
        {
            return RedirectToAction("Details", new { id, viewType });
        }

        // Вспомогательный метод для получения размеров продукта
        private float GetProductDimension(int productId, string dimensionType)
        {
            var product = _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null || product.PackagingTypes == null || !product.PackagingTypes.Any())
            {
                return 1.0f; // Значение по умолчанию
            }

            var packagingType = product.PackagingTypes.FirstOrDefault();
            if (packagingType == null)
            {
                return 1.0f; // Значение по умолчанию
            }

            switch (dimensionType)
            {
                case "Length":
                    return packagingType.Length;
                case "Width":
                    return packagingType.Width;
                case "Height":
                    return packagingType.Height;
                default:
                    return 1.0f; // Значение по умолчанию
            }
        }

        // Метод для расчета нагрузок на оси на основе размещения груза
        private Dictionary<string, float> CalculateAxleLoadsFromPlacements(Vehicle vehicle, List<CargoPlacement> placements)
        {
            if (vehicle == null)
            {
                throw new InvalidOperationException("Транспортное средство не найдено для схемы загрузки");
            }

            // Рассчитываем общий вес груза и его положение
            float totalCargoWeight = 0;
            float totalMoment = 0;

            foreach (var placement in placements)
            {
                // Получаем вес продукта
                float itemWeight = GetProductWeight(placement.ProductId);

                totalCargoWeight += itemWeight;

                // Рассчитываем момент (вес * расстояние от шкворня)
                // Предполагаем, что X - это расстояние от начала полуприцепа
                // Преобразуем его в расстояние от шкворня
                float distanceFromKingpin = (float)placement.X;
                totalMoment += itemWeight * distanceFromKingpin;
            }

            // Рассчитываем среднее положение центра тяжести груза
            float cargoPositionFromKingpin = 0;
            if (totalCargoWeight > 0)
            {
                cargoPositionFromKingpin = totalMoment / totalCargoWeight;
            }

            // Используем метод Vehicle для расчета нагрузок на оси
            return vehicle.CalculateAxleLoads(totalCargoWeight, cargoPositionFromKingpin);
        }

        // Вспомогательный метод для получения веса продукта
        private float GetProductWeight(int productId)
        {
            var product = _context.Products
                .Include(p => p.PackagingTypes)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null || product.PackagingTypes == null || !product.PackagingTypes.Any())
            {
                return 50.0f; // Значение по умолчанию
            }

            var packagingType = product.PackagingTypes.FirstOrDefault();
            if (packagingType == null)
            {
                return 50.0f; // Значение по умолчанию
            }

            // Предполагаем, что у PackagingType есть свойство Weight
            // Если нет, используем значение по умолчанию
            return packagingType.Weight > 0 ? packagingType.Weight : 50.0f;
        }
    }

    public class ViewPreferenceModel
    {
        public int SchemeId { get; set; }
        public string ViewType { get; set; }
    }
}
