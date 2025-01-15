using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ViewModels
{
    public class AdminUser
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
