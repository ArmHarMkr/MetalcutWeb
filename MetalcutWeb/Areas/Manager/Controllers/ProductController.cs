using MetalcutWeb.Areas.Manager.ViewModel;
using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace MetalcutWeb.Areas.Manager.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [Route("ManageProduct/[controller]/[action]")]
    [Area("Manager")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        private readonly IDeleteReferences<ProductEntity> _deleteProductReferences;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _host;

        public ProductController(IUnitOfWork unitOfWork, IDeleteReferences<ProductEntity> deleteProductReferences,
            UserManager<AppUser> userManager, AppDbContext db, IEmailSender emailSender,
            IWebHostEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _deleteProductReferences = deleteProductReferences;
            _userManager  = userManager;
            _db = db;
            _emailSender = emailSender;
            _host = host;
        }

        [HttpGet("AllProducts")]
        public async Task<IActionResult> AllProducts()
        {
            ProductViewModel productVM = new();
            IEnumerable<ProductEntity> productFromDb = _unitOfWork.Product.GetAll().ToList();
            productVM.Products = productFromDb;
            return View(productVM);
        }

        [HttpGet("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost("CreateProduct")]

        public async Task<IActionResult> CreateProduct(ProductViewModel productVM)
        {

                string uniqueFileName = null;
                if (productVM.Image != null)
                {
                    string uploadsFolder = Path.Combine(_host.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + productVM.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productVM.Image.CopyToAsync(fileStream);
                    }
                }


                // Create a new product entity
                var newProduct = new ProductEntity
                {
                    ProdName = productVM.ProductEntity.ProdName,
                    ProdDescription = productVM.ProductEntity.ProdDescription,
                    ImagePath = uniqueFileName
                };

                // Add the new product to the database
                await _unitOfWork.Product.Add(newProduct);
                await _unitOfWork.Save();

                // Send an email notification
                AppUser? currentUser = await _userManager.GetUserAsync(User);
                _emailSender.SendEmail(currentUser.Email, "Adding new Product", "<html><body><h1>New Product added successfully<h1></body></html>", true);

                return RedirectToAction("AllProducts");
            
        }


        [HttpGet("EditProduct")]
        public async Task<IActionResult> EditProduct(string? id)
        {
            if (id != null)
            {
                var productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
                if (productFromDb != null)
                    return View(productFromDb);
            }
            return NotFound();
        }

        [HttpPost("EditProduct")]
        public async Task<IActionResult> EditProduct(ProductEntity product)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Product.Update(product);
                await _unitOfWork.Save();
                return RedirectToAction("AllProducts");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string? id)
        {
            if (id != null)
            {
                if(id == "PlasmaId")
                {
                    TempData["ErrorMessage"] = "You cannot delete plasma cutter";
                    return RedirectToAction("AllProducts");
                }
                else
                {
                    var productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
                    _unitOfWork.Product.Remove(productFromDb);
                    _deleteProductReferences.DeleteReferences(productFromDb);
                    await _unitOfWork.Save();
                    TempData["SuccessMessage"] = "Product deleted successfully";
                    return RedirectToAction("AllProducts");
                }
            }
            return NotFound();
        }

    }
}

