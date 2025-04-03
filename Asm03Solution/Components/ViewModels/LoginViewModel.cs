using System.ComponentModel.DataAnnotations;

namespace Asm03Solution.Components.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password")]
        public string PassWord { get; set; }
    }
}
