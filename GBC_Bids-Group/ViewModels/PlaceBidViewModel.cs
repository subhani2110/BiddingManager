using DataLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace GBC_Bids_Group.ViewModels
{
    public class PlaceBidViewModel
    {
        public Asset? Asset { get; set; }
        public decimal MaxBid { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public decimal MinBid { get; set; }
    }
}
