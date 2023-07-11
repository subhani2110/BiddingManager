using DataLayer;
using DataLayer.Entities;
using GBC_Bids_Group.Helpers;
using GBC_Bids_Group.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GBC_Bids_Group.Controllers
{
    public class BidController : Controller
    {
        private readonly SessionManager _sessionManager;
        readonly IUnitOfWork _unitOfWork;
        public BidController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _sessionManager = new SessionManager(httpContextAccessor);
        }
        
        [HttpGet]
        public async Task<IActionResult> BidsByAsset(int id)
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }

            var bids = await _unitOfWork.BidRepository.GetBidsByAssetIdAsync(id);
            var asset = await _unitOfWork.AssetRepository.GetAssetByIdAsync(id);
            var vm = new BidsViewModel
            {
                Bids = bids.OrderByDescending(x => x.Date).ThenByDescending(x => x.AssetId).ThenByDescending(x => x.Amount),
                Asset = asset
            };

            return View(vm);
        }
        
        [HttpGet]
        public async Task<IActionResult> BidsByBuyer()
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }
            var userId = _sessionManager.GetUserId();

            var bids = await _unitOfWork.BidRepository.GetBidsByUserIdAsync(userId);
            var buyer = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var vm = new BidsViewModel
            {
                Bids = bids.OrderByDescending(x => x.Date).ThenByDescending(x => x.AssetId).ThenByDescending(x => x.Amount),
                Buyer = buyer
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AllBids()
        {
            if (!_sessionManager.IsAuthenticatedAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var bids = await _unitOfWork.BidRepository.GetAllBidsAsync();            
            
            var vm = new BidsViewModel
            {
                Bids = bids.OrderByDescending(x => x.Date).ThenByDescending(x => x.AssetId).ThenByDescending(x => x.Amount),
            };

            return View(vm);
        }
    }
}
