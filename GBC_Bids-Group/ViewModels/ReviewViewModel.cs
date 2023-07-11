using DataLayer.Entities;

namespace GBC_Bids_Group.ViewModels
{
    public class ReviewViewModel
    {
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public int AssetId { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
