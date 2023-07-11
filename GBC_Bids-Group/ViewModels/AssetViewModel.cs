using DataLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace GBC_Bids_Group.ViewModels
{
    public class AssetViewModel
    {
        public IEnumerable<Asset> Assets { get; set; }
        public bool IsUser { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinimumBid { get; set; }
        public string? Condition { get; set; }
        public string? Category { get; set; }
    }
}
