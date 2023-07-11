using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinimumBid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Condition { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSold { get; set; }
        public int SellerId { get; set; }

        public virtual User Seller { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public int SoldToId { get; set; }
        public DateTime SoldDate { get; set; }
    }

}
