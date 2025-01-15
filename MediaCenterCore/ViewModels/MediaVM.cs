using MediaCenterCore.Shared;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ViewModels
{
    public class MediaVM
    {
        public int Id { get; set; }

        public IFormFile? Photo { get; set; }

        public string? Image { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public required string Quality { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Name { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Description { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Language { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Max length of quality is 100")]
        public string Country { get; set; }

        [Required]
        [MinLength(1 , ErrorMessage ="Min length is 1")]
        [MinLengthForEach(5)]
        public IEnumerable<string> Actors { get; set; }

        [Required]
        [MinLength(1 , ErrorMessage ="Min length is 1")]
        [MinLengthForEach(5)]
        public IEnumerable<string> Directors { get; set; }

        [Required]
        [MinLength(1 , ErrorMessage ="Min length is 1")]
        public IEnumerable<CategoryVM>? Categories { get; set; }

    }
}
