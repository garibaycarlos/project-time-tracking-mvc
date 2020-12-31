using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTimeTracking.Data;
using ProjectTimeTracking.Models;

namespace ProjectTimeTracking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Category Category { get; set; }

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category = new Category();

            // create a new record
            if (id == null) return View(Category);

            // update an existing record
            Category = await _db.Category.FindAsync(id);

            if (Category == null) return NotFound();

            return View(Category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (ModelState.IsValid)
            {
                Category getCategory = _db.Category.AsNoTracking().FirstOrDefault(c => c.Name == Category.Name);

                if (Category.Id == 0) // it is a new record
                {
                    if (getCategory != null) // will not create a category if it already exists
                    {
                        TempData["message"] = "exists";

                        return View(Category);
                    }
                    else // create the record
                    {
                        Category.IsActive = true;
                        //Category.CreatedBy = this value is pending;
                        Category.DateCreated = DateTime.Now;

                        _db.Category.Add(Category);

                        TempData["message"] = "added";
                    }
                }
                else // update an existing record
                {
                    if (!Category.IsActive) // an inactive category will not be updated
                    {
                        return View(Category);
                    }

                    if (getCategory != null)
                    {
                        if (Category.Id != getCategory.Id) // category already exists for the update attempt
                        {
                            TempData["message"] = "exists";

                            return View(Category);
                        }
                    }

                    //Category.Modifiedby = User.Identity.Name;
                    Category.DateModified = DateTime.Now;

                    _db.Category.Update(Category);

                    TempData["message"] = "edited";
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(Category);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Category.ToListAsync() });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int id, bool isActive)
        {
            var customer = await _db.Category.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while changing status" });
            }

            //customer.Modifiedby = this value is pending;
            customer.DateModified = DateTime.Now;
            customer.IsActive = isActive;

            _db.Category.Update(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Status changed successfully" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Category.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (customer.IsActive)
            {
                return Json(new { success = false, message = "Cannot delete an active category" });
            }

            _db.Category.Remove(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API Calls
    }
}