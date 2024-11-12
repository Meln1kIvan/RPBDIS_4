using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RPBDIS_4.Data;
using RPBDIS_4.Models;

namespace RPBDIS_4.Controllers
{
    public class MaintenanceSchedulesController : Controller
    {
        private readonly MonitoringContext _context;

        public MaintenanceSchedulesController(MonitoringContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ResponseCache(Duration = 280)]
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.MaintenanceSchedules
                .Include(ms => ms.Equipment)
                .Include(ms => ms.MaintenanceType)
                .Include(ms => ms.ResponsibleEmployee)
                .ToListAsync();

            return View(schedules);
        }

        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _context.MaintenanceSchedules
                .Include(ms => ms.Equipment)
                .Include(ms => ms.MaintenanceType)
                .Include(ms => ms.ResponsibleEmployee)
                .FirstOrDefaultAsync(ms => ms.ScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }
    }
}
