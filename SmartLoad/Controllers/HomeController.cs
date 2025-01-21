using Microsoft.AspNetCore.Mvc;
using SmartLoad.Models;

public class HomeController : Controller
{
    // Поле _context используется для взаимо действия с БД через EF Core
    private readonly ApplicationDbContext _context;

    //Инициализация _context 
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Метод, который возвращает представление для первой страницы (не взаимодействует с БД)
    public IActionResult Index()
    {
        return View();
    }

    //Метод, который получает список всех типов Тс из БД и передаёт их в представление 
    public IActionResult TypesOfVehicles()
    {
        var vehicleTypes = _context.VehicleTypes.ToList();
        return View(vehicleTypes);
    }

    //Возвращает пустую форму для добавления нового ТС
    public IActionResult AddVehicleType()
    {
        return View();
    }

    //Принемаем данные из формы и добавляем их в БД, сохраняем изменения и перенаправляем на страницу TypesOFVehicle
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

    //Получаем список всех продуктов из БД и передаём в представление
    public IActionResult Products()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    //Возращает пустую форму для добавления нового продукта
    public IActionResult AddProduct()
    {
        return View();
    }

    //Также как и с типами ТС, Получаем данныые из формы и записываем их в БД, и перенаправляем в Products
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

    //Получаем список всех типов упаковок и передаём их в представление
    public IActionResult PackagingTypes()
    {
        var packagingTypes = _context.PackagingTypes.ToList();
        return View(packagingTypes);
    }

    //Возвращаем пустую форму для добавления нового типа упаковки
    public IActionResult AddPackagingType()
    {
        return View();
    }

    //Принимаем данные из формы и записываем иих в БД, после перенаправляем на страницу PackagingTypes
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
