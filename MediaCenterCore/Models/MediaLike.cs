using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenterCore.Models
{
    public class MediaLike
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        public int MediaId { get; set; }
        [ForeignKey(nameof(MediaId))]
        public Media? Media { get; set; }
        public bool IsLike { get; set; }
    }
}
