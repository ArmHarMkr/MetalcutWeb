using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetalcutWeb.Domain.Enums;

namespace MetalcutWeb.Domain.Entity
{
    public class StorageProductEntity
    {
        [Key]
        public Guid StorageProductId { get; set; }
        [Required(ErrorMessage = "Type Storage Product Name")]
        public string StorageProductName { get; set; }
        [Required(ErrorMessage = "Type Storage Description")]
        public string StorageProductDescription { get; set;}
        [Required(ErrorMessage = "Type Storage Product's count")]
        public int StorageProdCount { get; set; } = 0;
        [Required(ErrorMessage = "Type Storage Product's type in Storage")]
        public StorageProductTypes StorageProductTypes { get; set; }
    }
}
