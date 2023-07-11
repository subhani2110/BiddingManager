using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public interface IUnitOfWork
    {
        IAssetRepository AssetRepository { get; }
        IUserRepository UserRepository { get; }
        IAuthRepository AuthRepository { get; }
        IBidRepository BidRepository { get; }
        IReviewRepository ReviewRepository { get; }
        void Dispose();
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IAuthRepository AuthRepository { get; }
        public IUserRepository UserRepository { get; }
        public IAssetRepository AssetRepository { get; }
        public IBidRepository BidRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        public UnitOfWork(AppDbContext dbContext, IAuthRepository authRepository, IAssetRepository assetRepository, IBidRepository bidRepository, IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            AuthRepository = authRepository;
            AssetRepository = assetRepository;
            BidRepository = bidRepository;
            ReviewRepository = reviewRepository;
            UserRepository = userRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}
