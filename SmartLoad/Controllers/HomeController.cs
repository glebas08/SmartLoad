using Microsoft.AspNetCore.Mvc;
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

    //�������� ������ ���� ��������� �� �� � ������� � �������������
    public IActionResult Products()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    //��������� ������ ����� ��� ���������� ������ ��������
    public IActionResult AddProduct()
    {
        return View();
    }

    //����� ��� � � ������ ��, �������� ������� �� ����� � ���������� �� � ��, � �������������� � Products
    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }
        return View(product);
    }

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
}
