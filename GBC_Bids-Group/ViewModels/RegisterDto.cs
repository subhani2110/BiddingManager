using System.ComponentModel.DataAnnotations;

namespace GBC_Bids_Group.ViewModels
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [Display(Name ="Full Name")]
        public string Name { get; set; }
        public bool CanBuy { get; set; }
        public bool CanSell { get; set; }
    }

}
