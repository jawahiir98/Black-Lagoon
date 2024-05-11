using BlackLagoon.Infrastructure.Data;
using BlackLagoon.Web.Models.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlackLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext db;
        public VillaNumberController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var villaNumbers = db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create() 
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomExists = db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomExists)
            {
                db.VillaNumbers.Add(obj.VillaNumber);
                db.SaveChanges();
                TempData["success"] = "Villa number added successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomExists) TempData["error"] = "The villa Number already exists.";

            obj.VillaList = db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);
        }
    }
}
