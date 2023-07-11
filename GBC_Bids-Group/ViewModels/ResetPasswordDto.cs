using System.ComponentModel.DataAnnotations;

namespace GBC_Bids_Group.ViewModels
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
