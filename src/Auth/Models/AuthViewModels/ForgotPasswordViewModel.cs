using System.ComponentModel.DataAnnotations;

namespace Auth.Models.AuthViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
