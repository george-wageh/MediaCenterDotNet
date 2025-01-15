using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.Models
{
    public class SeriesRelation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required int MediaId { get; set; }
        [Required, ForeignKey(nameof(MediaId))]
        public virtual required Media Media { get; set; }


    }
}
