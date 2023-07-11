using System.ComponentModel.DataAnnotations;

namespace GBC_Bids_Group.ViewModels
{
    public class MfaDto
    {
        [Required]
        public int Code { get; set; }
        public int UserId { get; set; }
    }
}
