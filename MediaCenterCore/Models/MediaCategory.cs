using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenterCore.Models
{
    public class MediaCategory
    {
        [Required]
        public int MediaId { get; set; }
        [ForeignKey(nameof(MediaId))]
        public virtual Media? Media { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
    }
}
