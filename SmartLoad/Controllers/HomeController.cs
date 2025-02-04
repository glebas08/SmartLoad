using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;

public class HomeController : Controller
{
    // ���� _context ������������ ��� ������ �������� � �� ����� EF Core
    private readonly ApplicationDbContext _context;

    //������������� _context 
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    //�����, ������� ���������� ������������� ��� ������ �������� (�� ��������������� � ��)
    public IActionResult Index()
    {
        return View();
    }

    //�����, ������� �������� ������ ���� ����� �� �� �� � ������� �� � ������������� 
    public IActionResult TypesOfVehicles()
    {
        var vehicleTypes = _context.VehicleTypes.ToList();
        return View(vehicleTypes);
    }

    #region ��

    //���������� ������ ����� ��� ���������� ������ ��
    public IActionResult AddVehicleType()
    {
        return View();
    }

    //��������� ������ �� ����� � ��������� �� � ��, ��������� ��������� � �������������� �� �������� TypesOFVehicle
    [HttpPost]
    public IActionResult AddVehicleType(VehicleType vehicleType)
    {
        if (ModelState.IsValid)
        {
            _context.VehicleTypes.Add(vehicleType);
            _context.SaveChanges();
            return RedirectToAction("TypesOfVehicles");
        }
        return View(vehicleType);
    }
    //����� ��� �������� �� �� ��
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
    //����� ��� ��������� ��
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
        if (ModelState.IsValid)
        {
            _context.VehicleTypes.Update(vehicleType);
            _context.SaveChanges();
            return RedirectToAction("TypesOfVehicles");
        }
        return View(vehicleType);
    }

    #endregion

    #region �������

    //�������� ������ ���� ��������� �� �� � ������� � �������������
    public IActionResult Products()
    {
        var products = _context.Products.Include(p => p.PackagingType).ToList();
        return View(products);
    }

    // ���������� ���������
    public IActionResult AddProduct()
    {
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name");
        return View();
    }

    //����� ��� ��������� ������ � ���� �������� �� ��� ��������������
    [HttpGet]
    public JsonResult GetPackagingType(int id)
    {
        var packagingType = _context.PackagingTypes.Find(id);
        if (packagingType == null)
        {
            return Json(null);
        }
        return Json(packagingType);
    }


    //����� ��� � � ������ ��, �������� ������� �� ����� � ���������� �� � ��, � �������������� � Products
    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        // ���������� ���� PackagingType
       
        #region �������� �� ��������� (�� ��������)
        //if (product.PackagingType == null)
        //{
        //    ModelState.AddModelError("PackagingTypeId", "��������� ��� �������� �� ������.");
        //}

        //if (!ModelState.IsValid)
        //{
        //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        //    {
        //        Console.WriteLine(error.ErrorMessage);
        //    }
        //    ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", product.PackagingTypeId);
        //    return View(product);
        //}
        #endregion
        try
        {
            product.PackagingType = _context.PackagingTypes.Find(product.PackagingTypeId);
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ ��� ���������� ��������: {ex.Message}");
            ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", product.PackagingTypeId);
            return View(product);
        }
    }


    public IActionResult EditProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", product.PackagingTypeId);
        return View(product);
    }

    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }
        ViewData["PackagingTypeId"] = new SelectList(_context.PackagingTypes, "Id", "Name", product.PackagingTypeId);
        return View(product);
    }

    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost, ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Products));
    }
    #endregion


    #region ��� ��������
    //�������� ������ ���� ����� �������� � ������� �� � �������������
    public IActionResult PackagingTypes()
    {
        var packagingTypes = _context.PackagingTypes.ToList();
        return View(packagingTypes);
    }

    //���������� ������ ����� ��� ���������� ������ ���� ��������
    public IActionResult AddPackagingType()
    {
        return View();
    }

    //��������� ������ �� ����� � ���������� ��� � ��, ����� �������������� �� �������� PackagingTypes
    [HttpPost]
    public IActionResult AddPackagingType(PackagingType packagingType)
    {
        if (ModelState.IsValid)
        {
            _context.PackagingTypes.Add(packagingType);
            _context.SaveChanges();
            return RedirectToAction("PackagingTypes");
        }
        return View(packagingType);
    }

    public IActionResult EditPackagingType(int id)
    {
        var packagingType = _context.PackagingTypes.Find(id);
        if (packagingType == null)
        {
            return NotFound();
        }
        return View(packagingType);
    }

    [HttpPost]
    public IActionResult EditPackagingType(PackagingType packagingType)
    {
        if (ModelState.IsValid)
        {
            _context.PackagingTypes.Update(packagingType);
            _context.SaveChanges();
            return RedirectToAction("PackagingTypes");
        }
        return View(packagingType);
    }

    public IActionResult DeletePackagingType(int id)
    {
        var packagingType = _context.PackagingTypes.Find(id);
        if (packagingType == null)
        {
            return NotFound();
        }
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
        return RedirectToAction(nameof(PackagingTypes));
    }
    #endregion


}
