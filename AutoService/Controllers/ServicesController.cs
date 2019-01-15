using AutoService.Data;
using AutoService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.Controllers
{
    public class ServicesController : Controller
    {

        private ApplicationDbContext _db;

        public ServicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get : Services/Create

        public IActionResult Create(int carId)
        {
            var model = new CarandServicesViewModel
            {
                CarObj = _db.Cars.FirstOrDefault(c => c.Id == carId),
                ServiceTypesObj = _db.ServiceTypes.ToList(),
                PastServiceObj = _db.Services.Where(s => s.CarId == carId).OrderByDescending(s => s.DateAdded).Take(5)
            };

            return View(model);
        }

        //Post : Services/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarandServicesViewModel model)
        {
            if(ModelState.IsValid)
            {
                model.NewServiceObj.CarId = model.CarObj.Id;
                model.NewServiceObj.DateAdded = DateTime.Now;
                _db.Add(model.NewServiceObj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new { carId = model.CarObj.Id });
            }

            var newModel = new CarandServicesViewModel
            {
                CarObj = _db.Cars.FirstOrDefault(c => c.Id == model.CarObj.Id),
                ServiceTypesObj = _db.ServiceTypes.ToList(),
                PastServiceObj = _db.Services.Where(s => s.CarId == model.CarObj.Id).OrderByDescending(s => s.DateAdded).Take(5)
            };

            return View(newModel);
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