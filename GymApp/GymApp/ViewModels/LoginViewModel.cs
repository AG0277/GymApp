using System.ComponentModel.DataAnnotations;

namespace GymApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email adress is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Passowrd is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
