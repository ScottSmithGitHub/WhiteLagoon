using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var VillaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(VillaNumbers);
        }

        public IActionResult Create()
        {
            var list = _unitOfWork.Villa.GetAll().ToList();
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = new SelectList(list, "Id", "Name")
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                //_villaService.CreateVilla(obj);                
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            if (roomNumberExists)
            {
                TempData["error"] = "The villa number already exists.";
            }

            var list = _unitOfWork.Villa.GetAll().ToList();
            obj.VillaList = new SelectList(list, "Id", "Name");

            return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
            var list = _unitOfWork.Villa.GetAll().ToList();
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = new SelectList(list, "Id", "Name"),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {         

            if (ModelState.IsValid)
            {
                //_villaService.CreateVilla(obj);                
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            var list = _unitOfWork.Villa.GetAll().ToList();
            villaNumberVM.VillaList = new SelectList(list, "Id", "Name");

            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            var list = _unitOfWork.Villa.GetAll().ToList();
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = new SelectList(list, "Id", "Name"),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber
                .Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete the villa number.";
            return View();
        }
    }
}
