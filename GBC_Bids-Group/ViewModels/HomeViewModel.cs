namespace GBC_Bids_Group.ViewModels
{
    public class HomeViewModel
    {
        public int TotalAssets { get; set; }
        public int TotalBids { get; set; }
        public int TotalWon { get; set; }
        public int TotalUsers { get; internal set; }
    }
}
