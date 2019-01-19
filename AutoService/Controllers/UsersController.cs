﻿using AutoService.Data;
using AutoService.Models;
using AutoService.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.Controllers
{
    [Authorize(Roles = SD.AdminEndUder)]
    public class UsersController : Controller
    {

        private ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string option = null, string search = null)
        {
            var users = _db.Users.ToList();

            if (option == "email" && search != null)
            {
                users = _db.Users.Where(u => u.Email.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {
                if (option == "name" && search != null)
                {
                    users = _db.Users.Where(u => u.FirstName.ToLower().Contains(search.ToLower())
                || u.LastName.ToLower().Contains(search.ToLower())).ToList();
                }
                else
                {
                    if (option == "phone" && search != null)
                    {
                        users = _db.Users.Where(u => u.PhoneNumber.ToLower().Contains(search.ToLower())).ToList();
                    }

                }
            }

            return View(users);
        }

        //GET Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            ApplicationUser user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        //GET Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            ApplicationUser user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", user);
            }
            else
            {
                var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

                if (userInDb == null)
                {
                    return NotFound();
                }
                else
                {
                    userInDb.FirstName = user.FirstName;
                    userInDb.LastName = user.LastName;
                    userInDb.PhoneNumber = user.PhoneNumber;
                    userInDb.City = user.City;
                    userInDb.PostalCode = user.PostalCode;
                    userInDb.Address = user.Address;

                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "Users");
                }

            }


        }
        //GET Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            ApplicationUser user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        //Post Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
            var cars = _db.Cars.Where(x => x.UserId == userInDb.Id);

            List<Car> listCar = cars.ToList();

            foreach(var car in listCar)
            {
                var services = _db.Services.Where(x => x.CarId == car.Id);

                _db.Services.RemoveRange(services);
            }

            _db.Cars.RemoveRange(cars);
            _db.Users.Remove(userInDb);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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