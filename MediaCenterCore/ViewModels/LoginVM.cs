using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.ViewModels
{
    public class LoginVM
    {
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me") ]
        public bool RememberMe { get; set; }
    }
}
