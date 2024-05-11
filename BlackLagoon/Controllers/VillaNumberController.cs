using BlackLagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

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
            var villaNumbers = db.VillaNumbers.ToList();
            return View(villaNumbers);
        }
    }
}
