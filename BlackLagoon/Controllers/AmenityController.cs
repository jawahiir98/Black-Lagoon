using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlackLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AmenityController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            unitOfWork = _unitOfWork;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            var Amenities = unitOfWork.Amenity.GetAll();
            return View(Amenities);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Amenity obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                unitOfWork.Amenity.Add(obj);
                unitOfWork.Save();
                TempData["success"] = "Amenity created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Could not create Amenity.";
                return View(obj);
            }
        }
        public IActionResult Update(int amenityId)
        {
            Amenity? obj = unitOfWork.Amenity.Get(u => u.Id == amenityId);
            if (obj == null) return RedirectToAction("Error", "Home");
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Amenity obj)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Amenity.Update(obj);
                unitOfWork.Save();
                TempData["success"] = "Amenity updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Could not update Amenity.";
                return View(obj);
            }
        }
        public IActionResult Delete(int amenityId)
        {
            Amenity? obj = unitOfWork.Amenity.Get(u => u.Id == amenityId);
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Amenity obj)
        {
            Amenity? objFromDb = unitOfWork.Amenity.Get(u => u.Id == obj.Id);
            if (obj == null)
            {
                TempData["error"] = "Could not delete Amenity.";
                return RedirectToAction("Error", "Home");
            }
            else
            {
                if (obj is not null)
                {
                    unitOfWork.Amenity.Remove(objFromDb);
                    unitOfWork.Save();
                    TempData["success"] = "Amenity deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Could not delete Amenity.";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
    }
}
