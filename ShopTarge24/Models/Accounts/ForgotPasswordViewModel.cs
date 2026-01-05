using System.ComponentModel.DataAnnotations;

namespace ShopTarge24.Models.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
