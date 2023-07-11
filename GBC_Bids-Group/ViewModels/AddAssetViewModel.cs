namespace GBC_Bids_Group.ViewModels
{
    public class AddAssetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinimumBid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Condition { get; set; }
        public string Category { get; set; }
        public IFormFile ImageUrl { get; set; }
        public bool IsSold { get; set; }
        public int SellerId { get; set; }
    }
}
