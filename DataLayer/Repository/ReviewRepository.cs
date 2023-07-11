using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IReviewRepository
    {
        Task<Review> AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsBySellerIdAsync(int sellerId);
    }

    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Review>> GetReviewsBySellerIdAsync(int sellerId)
        {
            return await _context.Reviews
                .Where(r => r.SellerId == sellerId)
                .ToListAsync();
        }
    }

}
