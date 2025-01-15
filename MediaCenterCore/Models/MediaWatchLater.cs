using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenterCore.Models
{
    public class MediaWatchLater
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        public int MediaId { get; set; }
        [ForeignKey(nameof(MediaId))]
        public Media? Media { get; set; }
        public DateTime? SaveDate { get; set; }
    }
}
