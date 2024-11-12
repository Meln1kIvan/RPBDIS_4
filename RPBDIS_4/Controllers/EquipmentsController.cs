using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RPBDIS_4.Data;
using RPBDIS_4.Models;

namespace RPBDIS_4.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly MonitoringContext _context;

        public EquipmentsController(MonitoringContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ResponseCache(Duration = 280)]
        public async Task<IActionResult> Index()
        {
            var equipments = await _context.Equipments.ToListAsync();
            return View(equipments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(e => e.EquipmentId == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }
    }
}
