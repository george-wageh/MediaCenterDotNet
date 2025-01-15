using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenterCore.Models
{
    [Table("MediaFav")]
    public class MediaAddFav
    {
        [Required]
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        [Required]
        public required int MediaId { get; set; }
        [ForeignKey(nameof(MediaId))]
        public virtual Media? Media { get; set; }
        [Required]
        public DateTime SaveDate { get; set; }
    }
}
