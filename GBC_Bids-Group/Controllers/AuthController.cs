using DataLayer;
using DataLayer.Entities;
using GBC_Bids_Group.Helpers;
using GBC_Bids_Group.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GBC_Bids_Group.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly SessionManager _sessionManager;        

        public AuthController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;       
            _sessionManager = new SessionManager(httpContextAccessor);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _unitOfWork.AuthRepository.LoginAsync(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            MfaCookiesHelper.SetCode(HttpContext, "123123");

            //MfaCookiesHelper.SetCode(HttpContext, randomNumber.ToString());
            //await _unitOfWork.AuthRepository.SendEmailAsync(user.Email, "MFA Code", $"Your MFA Code is {randomNumber}");
            TempData["UserId"] = user.Id;
            return RedirectToAction("MFA", "Auth");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = new DataLayer.Entities.User
                {
                    Username = registerDto.Username,
                    Password = registerDto.Password,
                    Email = registerDto.Email,
                    Role = "Client",
                    Name = registerDto.Name,
                    CanBuy = true,
                    CanSell = true,
                };
                var result = await _unitOfWork.AuthRepository.RegisterAsync(user, registerDto.Password);

                if (result is not null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                ModelState.AddModelError(string.Empty, "Please Try Again");
            }

            return View(registerDto);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _sessionManager.ClearSession();
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult MFA()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MFA(MfaDto mfaDto)
        {
            var MfaCode = MfaCookiesHelper.GetCode(HttpContext);

            if (MfaCode == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (mfaDto.Code.ToString() == MfaCode)
            {
                MfaCookiesHelper.ClearCode(HttpContext);

                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(mfaDto.UserId);
                _sessionManager.SetSession(user.Username, user.Role, user.Id);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Code");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto resetPassword)
        {
            _unitOfWork.AuthRepository.ResetPasswordAsync(resetPassword.Email);

            return RedirectToAction("Login", "Auth");            
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            if (!_sessionManager.IsAuthenticatedAdmin())
            {
                return RedirectToAction("Login", "Auth");
            }

            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();

            var vm = new UserViewModel
            {
                Users = users.Where(x => x.Role == "Client"),
            };

            return View(vm);
        }
    }
}
