using MetalcutWeb.Domain.Entity;

namespace MetalcutWeb.Areas.StorageManagement.ViewModels
{
    public class StorageViewModel
    {
        public StorageProductEntity StorageProductEntity { get; set; }
        public IEnumerable<StorageProductEntity> StorageProducts { get; set; }
        public int AddingAmount { get; set; }
        public int ReducingAmount { get; set; }
    }
}
