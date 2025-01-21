using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;

// Создание билдера приложения 
var builder = WebApplication.CreateBuilder(args);

//Добаввление стрки подключения кв сервисы
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// добавление сервисов в контейнер
builder.Services.AddControllersWithViews();

//Построение прилодения 
var app = builder.Build();

// Настройка конвейера обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Настройка промежуточного ПО
// Эти строки кода добавляют различные компоненты промежуточного ПО в конвейер обработки запросов:
//- `UseHttpsRedirection`: перенаправляет HTTP-запросы на HTTPS.
//- `UseStaticFiles`: обслуживает статические файлы (например, CSS, JavaScript).
//- `UseRouting`: добавляет поддержку маршрутизации.
//- `UseAuthorization`: добавляет поддержку авторизации.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Настройка маршрутизации (Этот код настраивает маршрут по умолчанию для контроллеров MVC.)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Запуск прилрожения 
app.Run();
