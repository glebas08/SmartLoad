using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Data;
using SmartLoad.Models;
using SmartLoad.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Настройка культуры для всего приложения
var cultureInfo = new CultureInfo("en-US"); // Используем культуру с точкой как разделителем
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Добавление строки подключения к базе данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление сервисов в контейнер
builder.Services.AddControllersWithViews();

// Регистрация сервиса для загрузки
builder.Services.AddScoped<SmartLoad.Services.LoadingService>();

builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Построение приложения
var app = builder.Build();

// Настройка конвейера обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
