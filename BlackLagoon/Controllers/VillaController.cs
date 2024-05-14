using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlackLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public VillaController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            var Villas = unitOfWork.Villa.GetAll();
            return View(Villas);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                unitOfWork.Villa.Add(obj);
                unitOfWork.Villa.Save();
                TempData["success"] = "Villa created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Could not create Villa.";
                return View(obj);
            }
        }
        public IActionResult Update(int villaId)
        {
            Villa? obj = unitOfWork.Villa.Get(u => u.Id == villaId);
            if (obj == null) return RedirectToAction("Error", "Home");
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Villa.Update(obj);
                unitOfWork.Villa.Save();
                TempData["success"] = "Villa updated successfully.";
                return RedirectToAction("Index");   
            }
            else
            {
                TempData["error"] = "Could not update Villa.";
                return View(obj);
            }
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = unitOfWork.Villa.Get(u => u.Id == villaId);
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = unitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (obj == null)
            {
                TempData["error"] = "Could not delete Villa.";
                return RedirectToAction("Error", "Home");
            }
            else
            {
                unitOfWork.Villa.Remove(objFromDb);
                unitOfWork.Villa.Save();
                TempData["success"] = "Villa deleted successfully.";
                return RedirectToAction("Index");
            }
        }
    }
}
