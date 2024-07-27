using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;

namespace MetalcutWeb.Areas.Manager.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [Route("ManageServices/{controller}/{action}")]
    [Area("Manager")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext Context;
        private readonly IDeleteReferences<ProductEntity> _deleteProductReferences;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        public ServiceController(IUnitOfWork unitOfWork, IDeleteReferences<ProductEntity> deleteProductReferences,
            UserManager<AppUser> userManager, AppDbContext db, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _deleteProductReferences = deleteProductReferences;
            _userManager = userManager;
            Context = db;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult GetAllServices()
        {
            return View(_unitOfWork.Service.GetAll());
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceEntity creatingService)
        {
            try
            {
                await _unitOfWork.Service.Add(creatingService);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = "Service has been created successfully";
                return RedirectToAction("GetAllServices");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("GetAllServices");
            }
        }

        [HttpGet]
        public IActionResult UpdateService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateService(ServiceEntity updatedService)
        {
            try
            {
                await _unitOfWork.Service.UpdateService(updatedService);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = "Service has been updated successfully";
                return RedirectToAction("GetAllServices");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("GetAllServices");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveService(string? id)
        {
            ServiceEntity service = await Context.Services.FirstOrDefaultAsync(x => x.Id == id);
            _unitOfWork.Service.Remove(service);    
            await _unitOfWork.Save();
            return RedirectToAction("GetAllServices");
        }
    }
}
