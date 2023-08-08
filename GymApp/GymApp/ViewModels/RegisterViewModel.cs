using System.ComponentModel.DataAnnotations;

namespace GymApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email adress is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passowrd is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
