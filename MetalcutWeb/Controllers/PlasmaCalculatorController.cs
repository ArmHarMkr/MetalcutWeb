using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace MetalcutWeb.Controllers
{
    public class PlasmaCalculatorController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public PlasmaCalculatorController(AppDbContext db, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> RequestPlasmaDelivery(string? id, PlasmaCalcViewModel plasmaCalculatorVM)
        {
            try
            {
                ProductEntity productEntity = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if(productEntity == null)
                {
                    return BadRequest("Product not found");
                }
                plasmaCalculatorVM.ProductEntity = productEntity;
                AppUser currentUser = await _userManager.GetUserAsync(User);
                double price = plasmaCalculatorVM.PlasmaCalcEntity.Price();
                Delivery delivery = new();
                delivery.Price = price;
                delivery.DeliveryProduct = plasmaCalculatorVM.ProductEntity;
                delivery.RequestedUser = currentUser;
                await _unitOfWork.Delivery.Add(delivery);
                await _unitOfWork.Save();
                TempData["SuccessMessage"] = "Delivery Requested Successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Delivery Requested Failed Successfully ;-) " + ex.Message;
            }            
            return RedirectToAction("AllDeliveries", "Delivery");
        }

    }
}
