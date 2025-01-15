using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ViewModels
{
    public class CommentVM
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100 , MinimumLength =2 , ErrorMessage = "Title must be length less than 100 and greater than 2")]
        public string Title { get; set; }
        [Required]
        [StringLength(100 , MinimumLength = 2 , ErrorMessage = "Comment must be length less than 100 and greater than 2")]
        public string Comment { get; set; }
        [Required]
        public int MediaId { get; set; }

        public string UserName { get; set; }
    }
}
