using DoAn.Areas.Admin.Repository;
using DoAn.Models;
using DoAn.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DoAn.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AddUserModel> _userManager;
        private readonly SignInManager<AddUserModel> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IEmailSender emailSender, SignInManager<AddUserModel> signInManager, UserManager<AddUserModel> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    TempData["success"] = "Đăng nhập thành công";
                    var receiver = "2100005423@nttu.edu.vn";
                    var subject = "Đăng nhập trên thiết bị thành công";
                    var message = "Bạn đã đăng nhập thành công, trải nghiệm dịch vụ nhé";
                    await _emailSender.SendEmailAsync(receiver, subject, message);
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Tên người dùng và mật khẩu không hợp lệ");
            }
            return View(loginVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new AddUserModel { UserName = user.Username, Email = user.Email };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo User thành công.";
                    return Redirect("/account/login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
