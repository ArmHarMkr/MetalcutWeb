using MetalcutWeb.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.DAL.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<ChatEntity> Chats { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<LikeEntity> Likes { get; set; }
        public DbSet<StorageProductEntity> StorageProducts { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntity>().HasData( //Adding initial products.
                new ProductEntity
                {
                    Id = "PlasmaId",
                    ProdName = "Metal cutting with plasmacutter",
                    ProdDescription = "Cutting thick metal with a plasma cutter. The price could be changed because of thickness",
                    Price = 200,
                    StarCount = 5
                }
            );
        }
    }
}