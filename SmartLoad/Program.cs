using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;

// �������� ������� ���������� 
var builder = WebApplication.CreateBuilder(args);

// ��������� �������� ��� ����� ����������
var cultureInfo = new CultureInfo("en-US"); // ���������� �������� � ������ ��� ������������
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


//����������� ����� ����������� �� �������
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� �������� � ���������
builder.Services.AddControllersWithViews();

//���������� ���������� 
var app = builder.Build();

// ��������� ��������� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// ��������� �������������� ��
// ��� ������ ���� ��������� ��������� ���������� �������������� �� � �������� ��������� ��������:
//- `UseHttpsRedirection`: �������������� HTTP-������� �� HTTPS.
//- `UseStaticFiles`: ����������� ����������� ����� (��������, CSS, JavaScript).
//- `UseRouting`: ��������� ��������� �������������.
//- `UseAuthorization`: ��������� ��������� �����������.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ��������� ������������� (���� ��� ����������� ������� �� ��������� ��� ������������ MVC.)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//������ ����������� 
app.Run();
