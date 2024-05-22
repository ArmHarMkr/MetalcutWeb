using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MetalcutWeb.Areas.Customers.Controllers
{

    [Authorize(Roles = "Admin, Manager, Employee")]
    [Area("Customers")]
    [Route("Customers/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext db, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<OrderEntity> allOrders = _unitOfWork.Order.GetAll();
            return View(allOrders);
        }

        [HttpGet]
        [Route("CreateOrder")]
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderEntity order)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Order.Add(order);
                await _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(string? id)
        {
            OrderEntity orderEntity = _unitOfWork.Order.Get(o => o.Id == id);
            if (orderEntity == null)
            {
                return NotFound("No order found");
            }
            _unitOfWork.Order.Remove(orderEntity);
            await _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(string? id)
        {
            OrderEntity orderEntity = _unitOfWork.Order.Get(o => o.Id == id);
            if (orderEntity == null)
            {
                return BadRequest("No order found");
            }
            return View(orderEntity);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(OrderEntity order)
        {
            if (ModelState.IsValid)
            {
                _db.Update(order);
                await _unitOfWork.Save();
                return RedirectToAction("EditOrder");
            }
            return View();
        }
    }
}
