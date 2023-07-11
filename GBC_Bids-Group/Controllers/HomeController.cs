using DataLayer;
using GBC_Bids_Group.Helpers;
using GBC_Bids_Group.Models;
using GBC_Bids_Group.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GBC_Bids_Group.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _sessionManager = new SessionManager(httpContextAccessor);
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            if (_sessionManager.IsAuthenticatedAdmin())
            {
                return RedirectToAction("Admin");
            }
            else if (_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Client");
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public async Task<IActionResult> Client()
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }
            var userId = _sessionManager.GetUserId();
            var clientUsername = _sessionManager.GetUsername();
            ViewData["Username"] = $"Welcome, {clientUsername}!";
            

            var TotalAssets = await _unitOfWork.AssetRepository.GetAssetsBySellerIdAsync(userId);

            var TotalBids = await _unitOfWork.BidRepository.GetBidsByUserIdAsync(userId);

            var TotalUsers = await _unitOfWork.UserRepository.GetAllUsersAsync();

            var vm = new HomeViewModel()
            {
                TotalAssets = TotalAssets.Count(),
                TotalBids = TotalBids.Count(),
                TotalUsers = TotalUsers.Count(),
                TotalWon = TotalAssets.Where(x=>x.SoldToId== userId).Count()
            };

            return View(vm);
        }

        public async Task<IActionResult> Admin()
        {
            if (!_sessionManager.IsAuthenticatedAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var clientUsername = _sessionManager.GetUsername();
            ViewData["Username"] = $"Welcome, {clientUsername}!";
            var userId = _sessionManager.GetUserId();

            var TotalAssets = await _unitOfWork.AssetRepository.GetAllAssetsAsync();

            var TotalBids = await _unitOfWork.BidRepository.GetAllBidsAsync();

            var TotalUsers = await _unitOfWork.UserRepository.GetAllUsersAsync();

            var vm = new HomeViewModel()
            {
                TotalAssets = TotalAssets.Count(),
                TotalBids = TotalBids.Count(),
                TotalUsers = TotalUsers.Count(),
                TotalWon = TotalAssets.Where(x => x.SoldToId == userId).Count()
            };

            return View(vm);
        }
    }
}