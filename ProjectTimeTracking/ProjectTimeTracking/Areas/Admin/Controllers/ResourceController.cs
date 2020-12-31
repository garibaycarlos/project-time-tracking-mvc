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
    public class ResourceController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Resource Resource { get; set; }

        public ResourceController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Resource = new Resource();

            // create a new record
            if (id == null) return View(Resource);

            // update an existing record
            Resource = await _db.Resource.FindAsync(id);

            if (Resource == null) return NotFound();

            return View(Resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (ModelState.IsValid)
            {
                Resource getResource = _db.Resource.AsNoTracking().FirstOrDefault(r => r.FirstName == Resource.FirstName &&
                                                                                       r.LastName == Resource.LastName);

                if (Resource.Id == 0) // it is a new record
                {
                    if (getResource != null) // will not create a resource if it already exists
                    {
                        TempData["message"] = "exists";

                        return View(Resource);
                    }
                    else // create the record
                    {
                        bool emailExists = _db.Resource.FirstOrDefault(r => r.Email == Resource.Email) != null;

                        if (emailExists)
                        {
                            TempData["message"] = "email exists";

                            return View(Resource);
                        }

                        Resource.IsActive = true;
                        //Resource.CreatedBy = this value is pending;
                        Resource.DateCreated = DateTime.Now;

                        _db.Resource.Add(Resource);

                        TempData["message"] = "added";
                    }
                }
                else // update an existing record
                {
                    if (!Resource.IsActive) // an inactive resource will not be updated
                    {
                        return View(Resource);
                    }

                    if (getResource != null)
                    {
                        if (Resource.Id != getResource.Id) // resource already exists for the update attempt
                        {
                            TempData["message"] = "exists";

                            return View(Resource);
                        }
                    }

                    Resource getByEmail = _db.Resource.AsNoTracking().FirstOrDefault(r => r.Email == Resource.Email);

                    if (getByEmail.Id != Resource.Id) // email already exists for the update attempt
                    {
                        TempData["message"] = "email exists";

                        return View(Resource);
                    }

                    //Resource.Modifiedby = User.Identity.Name;
                    Resource.DateModified = DateTime.Now;

                    _db.Resource.Update(Resource);

                    TempData["message"] = "edited";
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(Resource);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Resource.ToListAsync() });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int id, bool isActive)
        {
            var customer = await _db.Resource.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while changing status" });
            }

            //customer.Modifiedby = this value is pending;
            customer.DateModified = DateTime.Now;
            customer.IsActive = isActive;

            _db.Resource.Update(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Status changed successfully" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Resource.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (customer.IsActive)
            {
                return Json(new { success = false, message = "Cannot delete an active resource" });
            }

            _db.Resource.Remove(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API Calls
    }
}