using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;

// �������� ������� ���������� 
var builder = WebApplication.CreateBuilder(args);

//����������� ����� ����������� �� �������
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� �������� � ���������
builder.Services.AddControllersWithViews();

//���������� ���������� 
var app = builder.Build();

// ��������� ��������� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
