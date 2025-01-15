using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ModelsNotMapped
{
    public class Rate
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Comment { get; set; }

        [Required, Range(0, 5)]
        public required byte RateVal { get; set; }
    }
}
