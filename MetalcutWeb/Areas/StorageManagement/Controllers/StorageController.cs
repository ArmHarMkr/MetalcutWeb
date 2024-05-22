using MetalcutWeb.Areas.StorageManagement.ViewModels;
using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MetalcutWeb.Areas.StorageManagement.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [Area("StorageManagement")]
    public class StorageController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public StorageController(AppDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            var allStorageProducts = _unitOfWork.StorageProduct.GetAll();
            StorageViewModel storageViewModel = new();
            storageViewModel.StorageProducts = allStorageProducts;
            return View(storageViewModel);
        }

        [HttpGet("AddStorageProduct")]
        public IActionResult AddStorageProduct()
        {
            ViewBag.StorageProductTypes = Enum.GetNames(typeof(StorageProductTypes)).ToList();
            return View();
        }

        [HttpPost("AddStorageProduct")]
        public async Task<IActionResult> AddStorageProduct(StorageProductEntity storageProductEntity)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.StorageProduct.Add(storageProductEntity);
                await _unitOfWork.Save();
                TempData["SuccessMessage"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();  
        }

        [HttpPost("AddToQuantity")]
        public async Task<IActionResult> AddToQuantity(string? id, StorageViewModel storageViewModel)
        {
            if(id != null)
            {
                StorageProductEntity storageProductEntity = _unitOfWork.StorageProduct.Get(p => p.StorageProductId.ToString() == id);
                if(storageProductEntity != null)
                {
                    storageProductEntity.StorageProdCount += storageViewModel.AddingAmount;
                    _db.Update(storageProductEntity);
                    await _unitOfWork.Save();
                }
                else
                {
                    TempData["ErrorMessage"] = "No Product found";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("ReduceFromQuantity")]
        public async Task<IActionResult> ReduceFromQuantity(string? id, StorageViewModel storageViewModel)
        {
            if (id != null)
            {
                StorageProductEntity storageProductEntity = _unitOfWork.StorageProduct.Get(p => p.StorageProductId.ToString() == id);
                if (storageProductEntity != null)
                {
                    if(storageViewModel.ReducingAmount > storageProductEntity.StorageProdCount)
                    {
                        storageProductEntity.StorageProdCount = 0;
                    }
                    storageProductEntity.StorageProdCount -= storageViewModel.ReducingAmount;
                    _db.Update(storageProductEntity);
                    await _unitOfWork.Save();
                }
                else
                {
                    TempData["ErrorMessage"] = "No Product found";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("DeleteProductFromStorage")]
        public async Task<IActionResult> DeleteProductFromStorage(string? id)
        {
            if(id != null)
            {
                StorageProductEntity storageProductEntity = _unitOfWork.StorageProduct.Get(p => p.StorageProductId.ToString() == id);
                if(storageProductEntity != null)
                {
                    _unitOfWork.StorageProduct.Remove(storageProductEntity);
                    await _unitOfWork.Save();
                }
                TempData["SuccessMessage"] = "Product was Successfully deleted";
            }
            return RedirectToAction("Index");
        }
    }
}
