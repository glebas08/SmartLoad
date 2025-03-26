using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;
using SmartLoad.Data;

namespace SmartLoad.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region ТС

        #region ТипТС

        public IActionResult TypesOfVehicles()
        {
            var vehicleTypes = _context.VehicleTypes.ToList();
            return View(vehicleTypes);
        }

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
    }
}
