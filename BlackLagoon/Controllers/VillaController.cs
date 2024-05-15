using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BlackLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public VillaController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            unitOfWork = _unitOfWork;
            webHostEnvironment = _webHostEnvironment;
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
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }
                unitOfWork.Villa.Add(obj);
                unitOfWork.Save();
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
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if (!string.IsNullOrEmpty(obj.ImageUrl)) {
                        string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                unitOfWork.Villa.Update(obj);
                unitOfWork.Save();
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
                unitOfWork.Save();
                TempData["success"] = "Villa deleted successfully.";
                return RedirectToAction("Index");
            }
        }
    }
}
