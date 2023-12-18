using Diana.Enum;
using Diana.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Diana.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };
            var resultEmail = await _userManager.FindByEmailAsync(registerVM.Email);
            if (resultEmail == null)
            {
                var result = await _userManager.CreateAsync(appUser, registerVM.Password);
               

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(registerVM);
                }
                return RedirectToAction(nameof(Index),"Home");
            }

            else
            {
                ModelState.AddModelError("Email", "Please use another email.");
                return View();
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username/Email or password is not valid!");
                    return View();
                }
            }
            var result = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true).Result;

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Please, try again after a while!");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username/Email or password is not valid!");
            }
            await _signInManager.SignInAsync(user, loginVM.RememberMe);
            if (returnUrl != null && !returnUrl.Contains("Login"))
            {
                return RedirectToAction(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //public async Task<IActionResult> CreateRole()
        //{
        //    foreach (var item in Enum.GetValues(typeof(UserRole)))
        //    {
        //        if (await _roleManager.FindByNameAsync(item.ToString()) == null)
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
        //        }
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
