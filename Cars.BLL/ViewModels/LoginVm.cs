using System.ComponentModel.DataAnnotations;

namespace Cars.BLL.ViewModels
{
    public class LoginVm
    {
        //[Required(ErrorMessage = "Email is required")] // use Fluent Validator
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}