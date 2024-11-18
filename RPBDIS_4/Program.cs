using Microsoft.EntityFrameworkCore;
using RPBDIS_4.Data;
using RPBDIS_4.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Подключаем DbContext с использованием строки подключения из конфигурации (appsettings.json)
builder.Services.AddDbContext<MonitoringContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем поддержку контроллеров и представлений
builder.Services.AddControllersWithViews();

// Добавляем распределённое кэширование для использования кэширования в контроллерах
builder.Services.AddDistributedMemoryCache();

// Настраиваем сессии, если необходимо
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Время жизни сессии
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Создаем объект приложения
var app = builder.Build();

// Настройка Middleware для обработки ошибок в режиме Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Настройка Middleware для инициализации базы данных
app.UseMiddleware<DatabaseInitializerMiddleware>();

// Middleware для обработки HTTPS редиректов и статических файлов
app.UseHttpsRedirection();
app.UseStaticFiles();

// Включаем поддержку сессий
app.UseSession();

// Включаем маршрутизацию
app.UseRouting();

// Настраиваем авторизацию (если требуется)
app.UseAuthorization();

// Определение стандартного маршрута для MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Запуск приложения
app.Run();
