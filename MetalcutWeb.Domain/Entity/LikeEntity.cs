using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.Domain.Entity
{
    public class LikeEntity
    {
        [Key]
        public string LikeId { get; set; } = Guid.NewGuid().ToString();
        public CommentEntity LikedComment { get; set; }
        public AppUser LikeUser { get; set;}
    }
}
