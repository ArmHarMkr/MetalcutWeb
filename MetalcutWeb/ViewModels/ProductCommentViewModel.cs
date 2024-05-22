using MetalcutWeb.Domain.Entity;

namespace MetalcutWeb.ViewModels
{
    public class ProductCommentViewModel
    {
        public CommentEntity CommentEntity { get; set; }
        public ProductEntity ProductEntity { get; set; }
        public IEnumerable<ProductEntity> Products { get; set; }
        public IEnumerable<CommentEntity> Comments { get; set; }    

    }

}
