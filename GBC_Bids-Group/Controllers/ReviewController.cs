using DataLayer;
using DataLayer.Entities;
using GBC_Bids_Group.Helpers;
using GBC_Bids_Group.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GBC_Bids_Group.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly SessionManager _sessionManager;
        public ReviewController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _sessionManager = new SessionManager(httpContextAccessor);
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            var asset = await _unitOfWork.AssetRepository.GetAssetByIdAsync(id);
            var seller = await _unitOfWork.UserRepository.GetUserByIdAsync(asset.SellerId);
            var vm = new ReviewViewModel
            {
                AssetId = id,
                Seller = seller.Name,
                SellerId = asset.SellerId,
                BuyerId = _sessionManager.GetUserId(),
                Buyer = _sessionManager.GetUsername(),
                Date = DateTime.Now,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReviewViewModel reviewView)
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }

            var review = new Review
            {
                BuyerId = reviewView.BuyerId,
                SellerId = reviewView.SellerId,
                Comment = reviewView.Comment,
                Rating = reviewView.Rating,
                Date = DateTime.Now,
            };

            await _unitOfWork.ReviewRepository.AddReviewAsync(review);

            return RedirectToAction("BidsByBuyer", "Bid", new { Id = _sessionManager.GetUserId() });
        }

        [HttpGet]
        public async Task<IActionResult> GetBySeller(int id)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetReviewsBySellerIdAsync(id);
            var seller = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            var vm = new ReviewViewModel
            {
                Reviews = reviews,
                Seller = seller.Name
            };
            return View(vm);
        }


    }
}
