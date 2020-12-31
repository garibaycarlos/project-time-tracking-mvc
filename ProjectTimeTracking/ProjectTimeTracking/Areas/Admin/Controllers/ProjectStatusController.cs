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
    public class ProjectStatusController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProjectStatus ProjectStatus { get; set; }

        public ProjectStatusController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProjectStatus = new ProjectStatus();

            // create a new record
            if (id == null) return View(ProjectStatus);

            // update an existing record
            ProjectStatus = await _db.ProjectStatus.FindAsync(id);

            if (ProjectStatus == null) return NotFound();

            return View(ProjectStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (ModelState.IsValid)
            {
                ProjectStatus getProjectStatus = _db.ProjectStatus.AsNoTracking().FirstOrDefault(p => p.Name == ProjectStatus.Name);

                if (ProjectStatus.Id == 0) // it is a new record
                {
                    if (getProjectStatus != null) // will not create a project status if it already exists
                    {
                        TempData["message"] = "exists";

                        return View(ProjectStatus);
                    }
                    else // create the record
                    {
                        ProjectStatus.IsActive = true;
                        //ProjectStatus.CreatedBy = this value is pending;
                        ProjectStatus.DateCreated = DateTime.Now;

                        _db.ProjectStatus.Add(ProjectStatus);

                        TempData["message"] = "added";
                    }
                }
                else // update an existing record
                {
                    if (!ProjectStatus.IsActive) // an inactive project status will not be updated
                    {
                        return View(ProjectStatus);
                    }

                    if (getProjectStatus != null)
                    {
                        if (ProjectStatus.Id != getProjectStatus.Id) // project status already exists for the update attempt
                        {
                            TempData["message"] = "exists";

                            return View(ProjectStatus);
                        }
                    }

                    //ProjectStatus.Modifiedby = User.Identity.Name;
                    ProjectStatus.DateModified = DateTime.Now;

                    _db.ProjectStatus.Update(ProjectStatus);

                    TempData["message"] = "edited";
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ProjectStatus);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.ProjectStatus.ToListAsync() });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int id, bool isActive)
        {
            var customer = await _db.ProjectStatus.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while changing status" });
            }

            //customer.Modifiedby = this value is pending;
            customer.DateModified = DateTime.Now;
            customer.IsActive = isActive;

            _db.ProjectStatus.Update(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Status changed successfully" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.ProjectStatus.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (customer.IsActive)
            {
                return Json(new { success = false, message = "Cannot delete an active project status" });
            }

            _db.ProjectStatus.Remove(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API Calls
    }
}