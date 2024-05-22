using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.DAL.Implementations
{
    public class CommentRepository : Repository<CommentEntity>, ICommentRepository
    {
        private readonly AppDbContext _db;
        public CommentRepository(AppDbContext db)
            :base(db)
        {
            _db = db;
        }
    }
}
