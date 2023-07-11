using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }

        public virtual Asset Asset { get; set; }
        public bool IsWon { get; set; }
    }
}
