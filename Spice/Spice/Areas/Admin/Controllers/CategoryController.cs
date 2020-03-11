using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.ManagerUser)]
    public class CategoryController : Controller
    {
        //local object
        private readonly ApplicationDbContext _db;

        //the object we are retrieving from our container using dependency injection
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET action method - retrieve everything from the database and pass it to the View
        public async Task<IActionResult> Index()
        {
            //pass all the category objects from the database NE
            //return everything from the Category
            //Recommend to use async and await
            return View(await _db.Category.ToListAsync());
        }

        //we will not pass anything to the View, View will be empty so we don't need to use async Task
        //GET - CREATE
        //[Authorize(Roles = SD.ManagerUser)]
        public IActionResult Create()
        {
            return View();
        }

        //POST-CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = SD.ManagerUser)]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //if valid
                _db.Category.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //based on Id, Async will retrieve the category inside the Caregory variable right here
            var category = await _db.Category.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST-EDIT
        //THESE ARE MUST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //it will look for the category based on the id, because this is the primary key 
                //and it will update all other properties (for now we have only Name)
                _db.Update(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //if not valid, return back to theview and pass the category object so that's filled automatically
            return View(category);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //based on Id, Async will retrieve the category inside the Caregory variable right here
            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST - DELETE
        //same names, that's not possible, but that way ASP.NET will understand that's actually a DELETE method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            //retrieve the category from the database based on the id and store it in the variable
            var category = await _db.Category.FindAsync(id);

            //if category exist
            if(category == null)
            {
                return View();
            }
            _db.Category.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}