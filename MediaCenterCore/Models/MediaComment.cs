using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.Models
{
    public class MediaComment
    {
        [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required int MediaId { get; set; }

        [ForeignKey(nameof(MediaId))]
        public virtual Media? Media { get; set; }

        [Required]
        public required string UserName { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Max length of Title is 50")]
        public required string Title { get; set; }

        [Required , MaxLength(200 , ErrorMessage = "Max length of Comment is 200")]
        public required string Comment { get; set; }

        [Required]
        public DateTime? CreationDate { get; set; }
    }
   
}
