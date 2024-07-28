using MetalcutWeb.DAL.Data;
using MetalcutWeb.Domain.Entity;
using MetalcutWeb.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.Service.Implementation
{
    public class ProductRefDeleter : IDeleteReferences<ProductEntity>
    {
        private readonly AppDbContext _db;
        public ProductRefDeleter(AppDbContext db)
        {
            _db = db;
        }

        private readonly string rootFolder = @"E:\.Net projects\MetalcutWeb\MetalcutWeb\wwwroot\images\";
        public async Task DeleteReferences(ProductEntity product)
        {
            IEnumerable<CommentEntity> comments = await _db.Comments
                .Include(c => c.Product)
                .Where(c => c.Product == product)
                .ToListAsync();

            foreach (CommentEntity comment in comments)
            {
                LikeEntity like = await _db.Likes
                    .Include(l => l.LikedComment)
                    .FirstOrDefaultAsync(l => l.LikedComment == comment);

                if (like != null)
                {
                    _db.Likes.Remove(like);
                }

                _db.Comments.Remove(comment);
            }
            IEnumerable<Delivery> deliveriesWithProducts = _db.Deliveries.Include(d => d.DeliveryProduct).Where(d => d.DeliveryProduct == product);
            File.Delete(Path.Combine(rootFolder, product.ImagePath));
            _db.Deliveries.RemoveRange(deliveriesWithProducts);
            await _db.SaveChangesAsync();
        }
    }

}
