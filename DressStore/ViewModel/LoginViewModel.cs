using DressStore.Global;
using System.ComponentModel.DataAnnotations;

namespace DressStore.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ErrorMessages.RequiredField)]
        public string Username { get; set; }
        [Required(ErrorMessage = ErrorMessages.RequiredField)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
