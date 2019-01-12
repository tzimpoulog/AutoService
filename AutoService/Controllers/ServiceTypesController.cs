using AutoService.Data;
using AutoService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.Controllers
{
    public class ServiceTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServiceTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET: ServiceTypes
        public IActionResult Index()
        {
            return View(_db.ServiceTypes.ToList());
        }

        //GET: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                _db.Add(serviceType);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(serviceType);
        }

        //Details : ServiceTypes/Details/1

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var serviceType = await _db.ServiceTypes.FirstOrDefaultAsync(m => m.Id == id);

            if(serviceType == null)
            {
                return NotFound();
            }

            return View(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}