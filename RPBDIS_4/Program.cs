using Microsoft.EntityFrameworkCore;
using RPBDIS_4.Data;
using RPBDIS_4.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ���������� DbContext � �������������� ������ ����������� �� ������������ (appsettings.json)
builder.Services.AddDbContext<MonitoringContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� ��������� ������������ � �������������
builder.Services.AddControllersWithViews();

// ��������� ������������� ����������� ��� ������������� ����������� � ������������
builder.Services.AddDistributedMemoryCache();

// ����������� ����������� � �������������� ������, ���� ����������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // ����� ����� ������
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ���������� middleware ��� ��������� ������ (� ������ production)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ��������� Middleware ��� ������������� ���� ������
app.UseMiddleware<RPBDIS_4.Middlewares.DatabaseInitializerMiddleware>();

// Middleware ��� ��������� ����������� ������, HTTPS ���������� � ������
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

// ���������� ������������� ��� MVC
app.UseRouting();

// ����������� ����������� (���� ���������)
app.UseAuthorization();

// ����������� ������������ �������� ��� MVC (�� ���������)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ������ ����������
app.Run();
