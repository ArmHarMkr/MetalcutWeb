using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.Domain.Entity
{
    public class CommentEntity
    {
        [Key]
        public string CommentId { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Type the comment, please")]
        public string CommentText { get; set; } 
        public AppUser CommentatorUser { get; set; }
        private int _stars;

        [Required(ErrorMessage = "Please enter a star rating.")]
        [Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5.")]
        public int Stars
        {
            get { return _stars; }
            set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Star rating must be between 1 and 5.");
                }

                _stars = value;
            }
        }
        public int LikeCount { get; set; } = 0;
        public DateTime CommentedTime { get; set; } = DateTime.Now;
        public ProductEntity Product { get; set; }
    }
}
