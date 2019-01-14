using AutoService.Data;
using AutoService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using AutoService.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Controllers
{
    public class CarsController : Controller
    {
        private ApplicationDbContext _db;

        public CarsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(string userid = null)
        {
            if (userid == null)
            {
                //Only called when a guest user logs in
                userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var model = new CarAndCustomerViewModel
            {
                Cars = _db.Cars.Where(c => c.UserId == userid),
                UserObj = _db.Users.FirstOrDefault(u => u.Id == userid)
            };


            return View(model);
        }


        //Create Get
        public IActionResult Create(string userId)
        {
            Car carObj = new Car
            {
                Year = DateTime.Now.Year,
                UserId = userId
            };
            return View(carObj);
        }

        //Create Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Add(car);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userId = car.UserId });
            }

            return View(car);

        }

        //Details Get
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.Cars
                .Include(c => c.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        //Edit Get
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.Cars
                .Include(c => c.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        //Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, Car car)
        {
            if (id != car.Id)
                return NotFound();

            if(ModelState.IsValid)
            {
                _db.Update(car);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userId = car.UserId });
            }

            return View(car);
        }

        //Delete Get
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.Cars
                .Include(c => c.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        //Edit Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _db.Cars.SingleOrDefaultAsync(c => c.Id == id);
            if (car == null)
                return NotFound();

            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { userId = car.UserId });
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