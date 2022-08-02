using System.ComponentModel.DataAnnotations;

namespace Cars.BLL.ViewModels
{
    public class  ChangePasswordVm
    {
        //[Required(ErrorMessage = "Email is required")] // use Fluent Validator
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string OldPassword { get; set; }

        //[Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }
    }
}