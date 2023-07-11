using DataLayer;
using DataLayer.Entities;
using GBC_Bids_Group.Helpers;
using GBC_Bids_Group.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GBC_Bids_Group.Controllers
{
    public class AssetController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly IUnitOfWork _unitOfWork;
        public AssetController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _sessionManager = new SessionManager(httpContextAccessor);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _unitOfWork.AssetRepository.GetAllAssetsAsync();

            var assetsToClose = list.Where(x => !x.IsSold && x.EndDate <= DateTime.Now && x.SoldToId >= 0);

            if (assetsToClose.Any())
            {
                foreach (var assest in assetsToClose)
                {
                    var bids = await _unitOfWork.BidRepository.GetBidsByAssetIdAsync(assest.Id);

                    if (bids.Any())
                    {
                        var maxBid = bids.Max(x => x.Amount);
                        var maxBidder = bids.MaxBy(x => x.Amount);
                        assest.IsSold = true;
                        assest.SoldToId = maxBidder.UserId;
                        assest.SoldDate = DateTime.Now;
                        await _unitOfWork.AssetRepository.UpdateAssetAsync(assest);
                        maxBidder.IsWon = true;
                        await _unitOfWork.BidRepository.UpdateBidAsync(maxBidder);
                    }
                    else
                    {
                        assest.SoldToId = -1;
                        assest.SoldDate = DateTime.Now;
                        await _unitOfWork.AssetRepository.UpdateAssetAsync(assest);
                    }
                }
            }

            list = await _unitOfWork.AssetRepository.GetAllAssetsAsync();

            var vm = new AssetViewModel
            {
                Assets = list,
                IsUser = _sessionManager.IsAuthenticated()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AssetViewModel assetView)
        {
            var list = await _unitOfWork.AssetRepository.GetAllAssetsAsync();

            list = list.Where(x => 
            x.Condition == assetView.Condition || 
            x.Category == assetView.Category || 
            x.Name == assetView.Name || 
            x.MinimumBid == assetView.MinimumBid ||
            x.Description == assetView.Description);

            var vm = new AssetViewModel
            {
                Assets = list,
                IsUser = _sessionManager.IsAuthenticated()
            };

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var asset = await _unitOfWork.AssetRepository.GetAssetByIdAsync(id);

            var bids = await _unitOfWork.BidRepository.GetBidsByAssetIdAsync(asset.Id);
            var currentBid = bids.Any() ? bids.Max(x => x.Amount) : 0;
            var vm = new PlaceBidViewModel
            {
                Asset = asset,
                MaxBid = currentBid,
                MinBid = currentBid > 0 ? currentBid + 1 : asset.MinimumBid
            };

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> AssetsBySeller(int id)
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }

            var list = await _unitOfWork.AssetRepository.GetAssetsBySellerIdAsync(id);

            var vm = new AssetViewModel
            {
                Assets = list
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AllAssets()
        {
            if (!_sessionManager.IsAuthenticatedAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var list = await _unitOfWork.AssetRepository.GetAllAssetsAsync();

            var vm = new AssetViewModel
            {
                Assets = list
            };

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> PlaceBid(int id)
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }

            var asset = await _unitOfWork.AssetRepository.GetAssetByIdAsync(id);

            var bids = await _unitOfWork.BidRepository.GetBidsByAssetIdAsync(asset.Id);
            var currentBid = bids.Any() ? bids.Max(x => x.Amount) : 0;
            var vm = new PlaceBidViewModel
            {
                Asset = asset,
                MaxBid = currentBid,
                MinBid = currentBid > 0 ? currentBid + 1 : asset.MinimumBid
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid(PlaceBidViewModel viewModel)
        {
            if(ModelState.IsValid == false)
            {
                return View(viewModel);
            }

            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }
            
            var bid = new Bid
            {
                Amount = viewModel.Amount,
                Date = DateTime.Now,
                UserId = _sessionManager.GetUserId(),
                AssetId = viewModel.AssetId
            };

            await _unitOfWork.BidRepository.AddBidAsync(bid);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAssetViewModel viewModel)
        {
            if (!_sessionManager.IsAuthenticatedClient())
            {
                return RedirectToAction("Login", "Auth");
            }

            var fileName = Path.GetFileName(viewModel.ImageUrl.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AssetImages", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await viewModel.ImageUrl.CopyToAsync(stream);
            }

            var newAsset = new Asset
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                MinimumBid = viewModel.MinimumBid,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                Condition = viewModel.Condition,
                Category = viewModel.Category,
                ImageUrl = fileName,
                IsSold = false,
                SellerId = _sessionManager.GetUserId()
            };

            await _unitOfWork.AssetRepository.AddAssetAsync(newAsset);

            return RedirectToAction("AssetsBySeller", new { id = _sessionManager.GetUserId() });
        }
    }
}
