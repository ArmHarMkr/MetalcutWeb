using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Service.Interface;
using MetalcutWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Configuration;

namespace MetalcutWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(AppDbContext db, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _db = db;   
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        public async Task<IActionResult> Index(string? searchTerm)
        {
            IEnumerable<ProductEntity> productsFromDb = await _db.Products.ToListAsync();
            ProductCommentViewModel productCommentViewModel = new ProductCommentViewModel();
            productCommentViewModel.Products = productsFromDb;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                productsFromDb = productsFromDb.Where(p => p.ProdName.ToLower().Contains(searchTerm));
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<CommentEntity> commentsFromDb = await _db.Comments
                                                           .Include(c => c.Product)
                                                           .Include(c => c.CommentatorUser)
                                                           .OrderByDescending(c => c.LikeCount)
                                                           .ToListAsync();
            productCommentViewModel.Comments = commentsFromDb;
            foreach (ProductEntity prod in productsFromDb)
            {
                IEnumerable<CommentEntity> prodComments = await _db.Comments
                                            .Include(c => c.Product)
                                            .Include(c => c.CommentatorUser)
                                            .Where(c => c.Product == prod)
                                            .ToListAsync();
                int prodStarCount = 0;
                foreach (var comment in prodComments)
                {
                    prodStarCount += comment.Stars;
                }
                try
                {
                    if (prodComments.Count() == 0)
                    {
                        prod.StarCount = 0;

                    }
                    else
                    {
                        prod.StarCount = prodStarCount / prodComments.Count();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }

            return View(productCommentViewModel);
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(string? id, ProductCommentViewModel productComVM)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            ProductEntity productFromDb = _unitOfWork.Product.Get(d => d.Id == id);
            if (productFromDb == null)
            {
                TempData["ErrorMessage"] = "Write for a particular product, please";
                return NotFound();
            }

            IEnumerable<CommentEntity> comments = await _db.Comments
                                                        .Include(c => c.Product)
                                                        .Include(c => c.CommentatorUser)
                                                        .Where(c => c.CommentatorUser == currentUser)
                                                        .Where(c => c.Product == productFromDb)
                                                        .ToListAsync();
            if(comments.Count() < 1)
            {
                CommentEntity addingComment = new CommentEntity
                {
                    CommentatorUser = currentUser,
                    Product = productFromDb,
                    CommentText = productComVM.CommentEntity.CommentText,
                    Stars = productComVM.CommentEntity.Stars
                };
                productComVM.CommentEntity = addingComment;
                productComVM.ProductEntity = productFromDb;
                await _unitOfWork.Comment.Add(addingComment);
                await _unitOfWork.Product.Update(productFromDb);
                await _unitOfWork.Save();
                TempData["SuccessMessage"] = "Comment added successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Every user can add only one comment";
            }

            return RedirectToAction("Index");
        }

        [HttpPost("DeleteComment")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string? id)
        {
            CommentEntity deletingComment = _unitOfWork.Comment.Get(c => c.CommentId == id);
            IEnumerable<LikeEntity> likes = _db.Likes.Include(d => d.LikedComment).Where(l => l.LikedComment == deletingComment);
            _db.Likes.RemoveRange(likes);
            await _unitOfWork.Save();         
            if(deletingComment == null) { return NotFound(); }
            _unitOfWork.Comment.Remove(deletingComment);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost("LikeComment")]
        [Authorize]
        public async Task<IActionResult> LikeComment(string? id)
        {
            CommentEntity? likingComment = await _db.Comments
                                                   .Include(c => c.CommentatorUser)
                                                   .Include(c => c.Product)
                                                   .FirstOrDefaultAsync(c => c.CommentId == id);
            if (likingComment == null)
            {
                return BadRequest("No Comment found");
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<LikeEntity> likes = await _db.Likes.Include(l => l.LikeUser).Include(l => l.LikedComment).ToListAsync();
            LikeEntity likeEntity = new();
            int userLikeInOneComment = 0;
            foreach (var like in likes)
            {
                if (like.LikeUser == currentUser && like.LikedComment == likingComment)
                {
                    userLikeInOneComment++;
                }
            }
            if (userLikeInOneComment == 0)
            {
                likeEntity.LikeUser = currentUser;
                likeEntity.LikedComment = likingComment;
                _db.Likes.Add(likeEntity);
                likingComment.LikeCount++;
                _db.Comments.Update(likingComment);
                await _unitOfWork.Save();
            }
            else
            {
                TempData["ErrorMessage"] = "One user can like the same comment only once";
            }
            return RedirectToAction("Index");

        }

    }
}