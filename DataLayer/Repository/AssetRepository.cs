using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public interface IAssetRepository
    {
        Task AddAssetAsync(Asset asset);
        Task DeleteAssetAsync(Asset asset);
        Task<IEnumerable<Asset>> GetAllAssetsAsync();
        Task<Asset> GetAssetByIdAsync(int id);
        Task<IEnumerable<Asset>> GetAssetsBySellerIdAsync(int sellerId);
        Task UpdateAssetAsync(Asset asset);
    }

    public class AssetRepository : IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            return await _context.Assets
                .Include(a => a.Seller)
                .Include(a => a.Bids)
                .ToListAsync();
        }

        public async Task<Asset> GetAssetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.Seller)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Asset>> GetAssetsBySellerIdAsync(int sellerId)
        {
            return await _context.Assets
                .Include(a => a.Seller)
                .Include(a => a.Bids)
                .Where(a => a.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task AddAssetAsync(Asset asset)
        {
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssetAsync(Asset asset)
        {
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
        }
    }
}
