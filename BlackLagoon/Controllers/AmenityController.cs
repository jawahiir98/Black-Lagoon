using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Repository;
using BlackLagoon.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var Amenities = unitOfWork.Amenity.GetAll(includeProperties:"Villa");
            return View(Amenities);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = unitOfWork.Villa.GetAll().Select(Villa => new SelectListItem
                {
                    Text = Villa.Name,
                    Value = Villa.Id.ToString()
                })
            };
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Amenity.Add(obj.Amenity);
                unitOfWork.Save();
                TempData["success"] = "Amenity created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                obj.VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem { 
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                TempData["error"] = "Could not create Amenity.";
                return View(obj);
            }
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Amenity.Update(obj.Amenity);
                unitOfWork.Save();
                TempData["success"] = "Amenity updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                obj.VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                TempData["error"] = "Could not update Amenity.";
                return View(obj);
            }
        }
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM obj)
        {
            Amenity? objFromDb = unitOfWork.Amenity.Get(u => u.Id == obj.Amenity.Id);
            if (objFromDb is not null)
            {
                unitOfWork.Amenity.Remove(objFromDb);
                unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }
    }
}
