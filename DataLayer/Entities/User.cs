using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool CanBuy { get; set; }
        public bool CanSell { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}
