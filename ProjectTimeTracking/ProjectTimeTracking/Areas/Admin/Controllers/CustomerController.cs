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
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Customer Customer { get; set; }

        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // update an existing record
            Customer = await _db.Customer.FindAsync(id);

            if (Customer == null) return NotFound();

            return View(Customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            if (ModelState.IsValid)
            {
                Customer getCustomer = _db.Customer.AsNoTracking().FirstOrDefault(c => c.Name == Customer.Name);

                if (!Customer.IsActive) // an inactive customer will not be updated
                {
                    return View(Customer);
                }

                if (getCustomer != null)
                {
                    if (Customer.Id != getCustomer.Id) // customer already exists for the update attempt
                    {
                        TempData["message"] = "exists";

                        return View(Customer);
                    }
                }

                //Customer.Modifiedby = User.Identity.Name;
                Customer.DateModified = DateTime.Now;

                _db.Customer.Update(Customer);

                TempData["message"] = "edited";

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(Customer);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Customer.ToListAsync() });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int id, bool isActive)
        {
            var customer = await _db.Customer.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while changing status" });
            }

            //customer.Modifiedby = this value is pending;
            customer.DateModified = DateTime.Now;
            customer.IsActive = isActive;

            _db.Customer.Update(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Status changed successfully" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Customer.FindAsync(id);

            if (customer == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (customer.IsActive)
            {
                return Json(new { success = false, message = "Cannot delete an active customer" });
            }

            _db.Customer.Remove(customer);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion API Calls
    }
}