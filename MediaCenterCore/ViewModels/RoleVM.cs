using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ViewModels
{
    public class RoleVM
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
