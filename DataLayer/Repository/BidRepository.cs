using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public interface IBidRepository
    {
        Task AddBidAsync(Bid bid);
        Task DeleteBidAsync(Bid bid);
        Task<IEnumerable<Bid>> GetAllBidsAsync();
        Task<Bid> GetBidByIdAsync(int id);
        Task<IEnumerable<Bid>> GetBidsByAssetIdAsync(int assetId);
        Task<IEnumerable<Bid>> GetBidsByUserIdAsync(int userId);
        Task UpdateBidAsync(Bid bid);
    }

    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;

        public BidRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetAllBidsAsync()
        {
            return await _context.Bids
                .Include(b => b.Asset)
                .ToListAsync();
        }

        public async Task<Bid> GetBidByIdAsync(int id)
        {
            return await _context.Bids
                .Include(b => b.Asset)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bid>> GetBidsByAssetIdAsync(int assetId)
        {
            return await _context.Bids
                .Include(b => b.Asset) 
                .Where(b => b.AssetId == assetId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetBidsByUserIdAsync(int userId)
        {
            return await _context.Bids
                .Include(b => b.Asset)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task AddBidAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBidAsync(Bid bid)
        {
            _context.Bids.Update(bid);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBidAsync(Bid bid)
        {
            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
        }
    }
}
