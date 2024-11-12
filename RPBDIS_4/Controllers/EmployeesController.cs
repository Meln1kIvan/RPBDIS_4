using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RPBDIS_4.Data;
using RPBDIS_4.Models;

namespace RPBDIS_4.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MonitoringContext _context;

        public EmployeesController(MonitoringContext context)
        {
            _context = context;
        }

        // Метод для отображения списка всех сотрудников
        [HttpGet]
        [ResponseCache(Duration = 280)] // Кэшируем результат на 280 секунд
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.CompletedWorks)
                    .ThenInclude(cw => cw.Equipment)
                .Include(e => e.CompletedWorks)
                    .ThenInclude(cw => cw.MaintenanceType)
                .Include(e => e.MaintenanceSchedules)
                    .ThenInclude(ms => ms.Equipment)
                .Include(e => e.MaintenanceSchedules)
                    .ThenInclude(ms => ms.MaintenanceType)
                .AsNoTracking()
                .FirstOrDefaultAsync(emp => emp.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Фильтрация дублей в коллекциях
            employee.CompletedWorks = employee.CompletedWorks
                .GroupBy(cw => new { cw.CompletedMaintenanceId, cw.ActualCost })
                .Select(g => g.First())
                .ToList();

            employee.MaintenanceSchedules = employee.MaintenanceSchedules
                .GroupBy(ms => new { ms.ScheduleId, ms.EstimatedCost })
                .Select(g => g.First())
                .ToList();

            return View(employee);
        }


    }
}
