using DataLayer.Entities;

namespace GBC_Bids_Group.ViewModels
{
    public class BidsViewModel
    {
        public IEnumerable<Bid> Bids { get; set; }
        public Asset? Asset { get; set; }
        public User? Buyer { get;  set; }
    }
}
