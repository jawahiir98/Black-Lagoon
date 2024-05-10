using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlackLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext db;
        public VillaController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var Villas = db.Villas.ToList();
            return View(Villas);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (ModelState.IsValid)
            {
                db.Villas.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }
    }
}
