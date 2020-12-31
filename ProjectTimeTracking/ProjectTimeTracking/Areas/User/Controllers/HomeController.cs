using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectTimeTracking.Data;
using ProjectTimeTracking.Models;
using ProjectTimeTracking.Models.ViewModels;

namespace ProjectTimeTracking.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProjectTrackerViewModel ProjectTrackerViewModel { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;

            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET
        public async Task<IActionResult> Upsert(int? id)
        {
            ProjectTrackerViewModel = new ProjectTrackerViewModel()
            {
                CategoryList = await _db.Category.Where(c => c.IsActive == true).OrderBy(c => c.Name).ToListAsync(),
                ResourceList = await _db.Resource.Where(r => r.IsActive == true).OrderBy(r => r.FirstName).ToListAsync(),
                ProjectStatusList = await _db.ProjectStatus.Where(p => p.IsActive == true).OrderBy(p => p.Name).ToListAsync(),
                ProjectTracker = new ProjectTracker()
            };

            // create a new record
            if (id == null) return View(ProjectTrackerViewModel);

            // update an existing record
            ProjectTrackerViewModel.ProjectTracker = await _db.ProjectTracker.FindAsync(id);
            
            // this value needs to be set to null before SaveChangesAsync
            ProjectTrackerViewModel.ProjectTracker.Customer = await _db.Customer.FindAsync(ProjectTrackerViewModel.ProjectTracker.CustomerId); 

            if (ProjectTrackerViewModel.ProjectTracker == null) return NotFound();

            return View(ProjectTrackerViewModel);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(/*ProjectTrackerViewModel model*/)
        {
            var modelVM = new ProjectTrackerViewModel()
            {
                CategoryList = await _db.Category.Where(c => c.IsActive == true).OrderBy(c => c.Name).ToListAsync(),
                ResourceList = await _db.Resource.Where(r => r.IsActive == true).OrderBy(r => r.FirstName).ToListAsync(),
                ProjectStatusList = await _db.ProjectStatus.Where(p => p.IsActive == true).OrderBy(p => p.Name).ToListAsync(),
                ProjectTracker = ProjectTrackerViewModel.ProjectTracker
            };

            if (ModelState.IsValid)
            {
                ProjectTracker getProject = _db.ProjectTracker.AsNoTracking().FirstOrDefault(p => p.Category.Id == ProjectTrackerViewModel.ProjectTracker.CategoryId &&
                                                                                                  p.Customer.Id == ProjectTrackerViewModel.ProjectTracker.CustomerId &&
                                                                                                  p.Resource.Id == ProjectTrackerViewModel.ProjectTracker.ResourceId &&
                                                                                                  p.Initiator == ProjectTrackerViewModel.ProjectTracker.Initiator &&
                                                                                                  p.Description == ProjectTrackerViewModel.ProjectTracker.Description);

                if (ProjectTrackerViewModel.ProjectTracker.Id == 0) // it is a new record
                {
                    if (getProject != null) // will not create a category if it already exists
                    {
                        TempData["message"] = "exists";

                        return View(modelVM);
                    }
                    else // create the record
                    {
                        ProjectTrackerViewModel.ProjectTracker.Customer = null;
                        //ProjectTrackerViewModel.ProjectTracker.CreatedBy = this value is pending;
                        ProjectTrackerViewModel.ProjectTracker.DateCreated = DateTime.Now;

                        _db.ProjectTracker.Add(ProjectTrackerViewModel.ProjectTracker);

                        TempData["message"] = "added";
                    }
                }
                else // update an existing record
                {
                    if (getProject != null)
                    {
                        if (ProjectTrackerViewModel.ProjectTracker.Id != getProject.Id) // category already exists for the update attempt
                        {
                            TempData["message"] = "exists";

                            return View(modelVM);
                        }
                    }

                    ProjectTrackerViewModel.ProjectTracker.Customer = null;
                    //ProjectTrackerViewModel.ProjectTracker.Modifiedby = User.Identity.Name;
                    ProjectTrackerViewModel.ProjectTracker.DateModified = DateTime.Now;

                    _db.ProjectTracker.Update(ProjectTrackerViewModel.ProjectTracker);

                    TempData["message"] = "edited";
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(modelVM);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new
            {
                data = await _db.ProjectTracker.Include(p => p.Category)
                                               .Include(p => p.Customer)
                                               .Include(p => p.Resource)
                                               .Include(p => p.ProjectStatus)
                                               .ToListAsync()
            });
        }

        [HttpGet]
        public IActionResult SearchCustomer(string term)
        {
            return Json(_db.Customer.Where(c => c.Name.Contains(term))
                                    .Select(c => new
                                    {
                                        c.Id,
                                        c.Name
                                    })
                                    .OrderBy(c => c.Name)
                                    .ToArray());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var projectTracker = await _db.ProjectTracker.FindAsync(id);

            if (projectTracker == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _db.ProjectTracker.Remove(projectTracker);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API Calls
    }
}