using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RPBDIS_4.Data;
using RPBDIS_4.Models;

namespace RPBDIS_4.Controllers
{
    public class CompletedWorksController : Controller
    {
        private readonly MonitoringContext _context;

        public CompletedWorksController(MonitoringContext context)
        {
            _context = context;
        }

        // GET: /CompletedWorks
        [HttpGet]
        [ResponseCache(Duration = 280)]
        public async Task<IActionResult> Index()
        {
            var completedWorks = await _context.CompletedWorks
                .Include(cw => cw.Equipment)         // Подгружаем связанное оборудование
                .Include(cw => cw.MaintenanceType)   // Подгружаем связанный тип обслуживания
                .Include(cw => cw.ResponsibleEmployee) // Подгружаем ответственного сотрудника
                .ToListAsync();

            return View(completedWorks);
        }

        // GET: /CompletedWorks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var completedWork = await _context.CompletedWorks
                .Include(cw => cw.Equipment)
                .Include(cw => cw.MaintenanceType)
                .Include(cw => cw.ResponsibleEmployee)
                .FirstOrDefaultAsync(cw => cw.CompletedMaintenanceId == id);

            if (completedWork == null)
            {
                return NotFound();
            }

            return View(completedWork);
        }
    }
}
