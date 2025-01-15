using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace MediaCenterCore.Models
{
    public class Media
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Image { get; set; } = "no-image.png";

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Quality { get; set; }
        public virtual ICollection<MediaCategory>? MediaCategories { get; set; }
        public virtual ICollection<MediaAddFav>? MediaFav { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Name { get; set; }
		[Required]
        public string Description { get; set; }

		[Required]
        public string Staff { get; set; }

        public DateTime? ReleaseDate { get; set; }
		[Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Language { get; set; }

		[Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Country { get; set; }


        public virtual ICollection<MediaLike>? MediaLike { get; set; }
        public virtual ICollection<MediaWatchLater>? MediaWatchLater { get; set; }
        public virtual ICollection<MediaComment>? MediaComment { get; set; }


        public int? ratesCount { get; set; }

        public int? ratesSum { get; set; }

        public float? ratesAvg { get; set; }


    }
}

