using System.ComponentModel.DataAnnotations;

namespace GBC_Bids_Group.ViewModels
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
