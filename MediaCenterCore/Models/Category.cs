using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(25, ErrorMessage = "Max length of Name is 25")]
        public required string Name { get; set; }

        public virtual ICollection<MediaCategory>? MediaCategories { get; set; }

    }
}
 