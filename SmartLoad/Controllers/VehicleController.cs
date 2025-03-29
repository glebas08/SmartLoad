using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;
using SmartLoad.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SmartLoad.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Транспортные средства

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegistrationNumber,Name,Notes,TractorModel,TractorEmptyWeight,TractorAxleCount,TractorFrontAxleType,TractorMaxFrontAxleLoad,TractorEmptyFrontAxleLoad,TractorRearAxleType,TractorMaxRearAxleLoad,TractorEmptyRearAxleLoad,TractorWheelBase,TractorRearAxleToKingpin,TractorMaxLoadCapacity,TrailerModel,TrailerEmptyWeight,TrailerLength,TrailerWidth,TrailerHeight,TrailerAxleCount,TrailerAxleType,TrailerMaxAxleLoad,TrailerKingpinToAxle,TrailerAxleSpread,TrailerMaxLoadCapacity,TrailerMaxVolumeCapacity")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,Name,Notes,TractorModel,TractorEmptyWeight,TractorAxleCount,TractorFrontAxleType,TractorMaxFrontAxleLoad,TractorEmptyFrontAxleLoad,TractorRearAxleType,TractorMaxRearAxleLoad,TractorEmptyRearAxleLoad,TractorWheelBase,TractorRearAxleToKingpin,TractorMaxLoadCapacity,TrailerModel,TrailerEmptyWeight,TrailerLength,TrailerWidth,TrailerHeight,TrailerAxleCount,TrailerAxleType,TrailerMaxAxleLoad,TrailerKingpinToAxle,TrailerAxleSpread,TrailerMaxLoadCapacity,TrailerMaxVolumeCapacity")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/CalculateAxleLoads/5
        public async Task<IActionResult> CalculateAxleLoads(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            // Инициализируем пустые значения для первого отображения
            ViewBag.VehicleId = id;
            ViewBag.CargoWeight = 0;
            ViewBag.CargoPosition = 0;

            // Инициализируем словарь с нулевыми значениями для всех ключей
            var axleLoads = new Dictionary<string, float>
            {
                ["TractorFrontAxle"] = 0,
                ["TractorRearAxle"] = 0,
                ["TrailerAxles"] = 0,
                //["KingpinLoad"] = 0
            };

            ViewBag.AxleLoads = axleLoads;
            ViewBag.IsValid = true; // По умолчанию для пустого груза

            return View(vehicle);
        }

        // POST: Vehicles/CalculateAxleLoads/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateAxleLoads(int id, float? cargoWeight = null, float? cargoPositionFromKingpin = null)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.VehicleId = id;
            ViewBag.CargoWeight = cargoWeight ?? 0;
            ViewBag.CargoPosition = cargoPositionFromKingpin ?? 0;

            Dictionary<string, float> axleLoads;

            // Если это POST запрос с параметрами
            if (cargoWeight != null && cargoPositionFromKingpin != null)
            {
                // Используем метод из модели Vehicle для расчета нагрузок
                axleLoads = vehicle.CalculateAxleLoads(cargoWeight.Value, cargoPositionFromKingpin.Value);

                // Проверка допустимых нагрузок с использованием метода из модели
                bool isValid = vehicle.ValidateAxleLoads(axleLoads);
                ViewBag.IsValid = isValid;
            }
            else
            {
                // Если параметры не переданы, инициализируем пустой словарь
                axleLoads = new Dictionary<string, float>
                {
                    ["TractorFrontAxle"] = 0,
                    ["TractorRearAxle"] = 0,
                    ["TrailerAxles"] = 0,
                    //["KingpinLoad"] = 0
                };
                ViewBag.IsValid = true; // По умолчанию для пустого груза
            }

            ViewBag.AxleLoads = axleLoads;

            return View(vehicle); // Возвращаем модель vehicle в представление
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }

        #endregion
    }
}
