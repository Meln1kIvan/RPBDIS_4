using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RPBDIS_4.Data;
using RPBDIS_4.Models;

namespace RPBDIS_4.Controllers
{
    public class MaintenanceTypesController : Controller
    {
        private readonly MonitoringContext _context;

        public MaintenanceTypesController(MonitoringContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ResponseCache(Duration = 280)]
        public async Task<IActionResult> Index()
        {
            var maintenanceTypes = await _context.MaintenanceTypes.ToListAsync();
            return View(maintenanceTypes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var maintenanceType = await _context.MaintenanceTypes
                .FirstOrDefaultAsync(mt => mt.MaintenanceTypeId == id);

            if (maintenanceType == null)
            {
                return NotFound();
            }

            return View(maintenanceType);
        }
    }
}
