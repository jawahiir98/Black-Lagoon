using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using BlackLagoon.Infrastructure.Repository;
using BlackLagoon.Web.Models.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlackLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public VillaNumberController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create() 
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
            bool roomExists = unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomExists)
            {
                unitOfWork.VillaNumber.Add(obj.VillaNumber);
                unitOfWork.Save();
                TempData["success"] = "Villa number added successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomExists) TempData["error"] = "The villa Number already exists.";

            obj.VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);
        }
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
}
        [HttpPost]
        public IActionResult Update(VillaNumberVM obj)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.VillaNumber.Update(obj.VillaNumber);
                unitOfWork.Save();
                TempData["success"] = "Villa number updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            obj.VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);
        }
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                unitOfWork.VillaNumber.Remove(objFromDb);
                unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Villa number could not be deleted.";
            return View();
        }
    }
}
