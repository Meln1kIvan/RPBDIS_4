using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using RPBDIS_4.Data; // Замените на ваше пространство имен для DbContext
using RPBDIS_4.Models; // Замените на ваше пространство имен для моделей
using System;
using System.Linq;

namespace RPBDIS_4.Middlewares // Замените на ваше пространство имен
{
    public class DatabaseInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public DatabaseInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Получаем ApplicationDbContext через DI
            var dbContext = context.RequestServices.GetRequiredService<MonitoringContext>();

            // Проверяем, пусты ли таблицы. Если да, то добавляем тестовые данные
            if (!dbContext.Equipments.Any())
            {
                dbContext.Equipments.AddRange(
                    new Equipment { InventoryNumber = "INV001", Name = "Pump", StartDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)), Location = "Plant 1" },
                    new Equipment { InventoryNumber = "INV002", Name = "Compressor", StartDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), Location = "Plant 2" }
                );

                dbContext.CompletedWorks.AddRange(
                    new CompletedWork { MaintenanceTypeId = 1, EquipmentId = 1, CompletionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), ResponsibleEmployeeId = 1, ActualCost = 150.00M },
                    new CompletedWork { MaintenanceTypeId = 2, EquipmentId = 2, CompletionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), ResponsibleEmployeeId = 2, ActualCost = 200.00M }
                );


                dbContext.MaintenanceTypes.AddRange(
                    new MaintenanceType { Description = "Routine Check" },
                    new MaintenanceType { Description = "Repair" }
                );

                dbContext.Employees.AddRange(
                    new Employee { FullName = "John Smith", Position = "Technician" },
                    new Employee { FullName = "Jane Doe", Position = "Engineer" }
                );


                await dbContext.SaveChangesAsync();
            }

            // Передаем запрос дальше по конвейеру
            await _next(context);
        }
    }

    // Класс-расширение для упрощения добавления middleware
    public static class DatabaseInitializerExtensions
    {
        public static IApplicationBuilder UseDatabaseInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DatabaseInitializerMiddleware>();
        }
    }
}
