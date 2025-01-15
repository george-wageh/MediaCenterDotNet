using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [DisplayName("Full name")]
        public string FullName { get; set; }

        [Required , EmailAddress]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
               ErrorMessage = "Password must be at least 8 characters long and include one uppercase letter, one number, and one special character.")]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }
    }
}
