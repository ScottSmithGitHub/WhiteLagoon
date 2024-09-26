using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        public IActionResult Create()
        {
            var list = _unitOfWork.Villa.GetAll().ToList();
            AmenityVM amenityVM = new()
            {
                VillaList = new SelectList(list, "Id", "Name")
            };

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                //_villaService.CreateVilla(obj);                
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "There was an error creating the amenity.";
            var list = _unitOfWork.Villa.GetAll().ToList();
            obj.VillaList = new SelectList(list, "Id", "Name");

            return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
            var list = _unitOfWork.Villa.GetAll().ToList();
            AmenityVM amenityVM = new()
            {
                VillaList = new SelectList(list, "Id", "Name"),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {

            if (ModelState.IsValid)
            {              
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            var list = _unitOfWork.Villa.GetAll().ToList();
            amenityVM.VillaList = new SelectList(list, "Id", "Name");

            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            var list = _unitOfWork.Villa.GetAll().ToList();
            AmenityVM amenityVM = new()
            {
                VillaList = new SelectList(list, "Id", "Name"),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }


        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitOfWork.Amenity
                .Get(u => u.Id == amenityVM.Amenity.Id);

            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete the amenity.";
            return View();
        }
    }
}
