using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MetalcutWeb.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public DeliveryController(AppDbContext db, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpPost("RequestDelivery")]
        public async Task<IActionResult> RequestDelivery(string? id)
        {
            var productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (productFromDb != null || currentUser != null)
            {
                try
                {
                    await _unitOfWork.Delivery.RequestDelivery(currentUser, productFromDb);
                    await _unitOfWork.Save();
                    //_emailSender.SendEmail(currentUser.Email, "Delivery", "<html><body>Delivery requested successfully</body></html>", true);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return RedirectToAction("AllDeliveries");
            }
            return NotFound();
        }

        public async Task<IActionResult> AllDeliveries()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<Delivery> deliveriesFromDb = _db.Deliveries
                                                        .Include(d => d.DeliveryProduct)
                                                        .Include(d => d.RequestedUser)
                                                        .Include(d => d.AcceptedUser)
                                                        .Where(p => p.RequestedUser == currentUser);
            return View(deliveriesFromDb);
        }

        [HttpPost("DeleteDeliveryRequest")]
        public async Task<IActionResult> DeleteDeliveryRequest(string? id)
        {
            Delivery deliveryFromDb = _unitOfWork.Delivery.Get(d => d.Id == id);
            _unitOfWork.Delivery.Remove(deliveryFromDb);
            await _unitOfWork.Save();
            return RedirectToAction("AllDeliveries");
        }
    }
}
