using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketProject.Models.Context;
using TicketProject.Models.Entities;
using TicketProject.Areas.Admin.Models.Entities;
using TicketProject.Areas.Models.Entities;

namespace TicketProject.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly TicketContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public HomeController(UserManager<AppUser> userManager, TicketContext context, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var user = _HttpContextAccessor.HttpContext.User.Identity.Name;
            if(user == null)
            {
                return RedirectToAction("SignIn");
            }
            return View();
        }


        public IActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminCreate admin)
        {
            AppUser adminuser = new()
            {
                UserName = admin.userName
            };


            var identityresult = await _userManager.CreateAsync(adminuser, admin.password);
            if (identityresult.Succeeded)
            {
                await _roleManager.CreateAsync(new()
                {
                    Name = "Admin"
                });
                await _roleManager.CreateAsync(new()
                {
                    Name = "Employee"
                });
                await _roleManager.CreateAsync(new()
                {
                    Name = "Customer"
                });

                return RedirectToAction("Index", "Home");
            }

            return View();

        }


        public IActionResult SignIn()
        {
            
            return View();
            
        }



        [HttpPost]

        public async Task<IActionResult> SignIn(AdminSignIn model)
        {
            if (ModelState.IsValid)
            {
                var SignInResult = await _signInManager.PasswordSignInAsync(model.adminUserName, model.adminPassword, false, true);
                if (SignInResult.Succeeded)
                {

                }
                else if (SignInResult.IsLockedOut)
                {

                }
                else if (SignInResult.IsNotAllowed)
                {

                }
                
            }

            return RedirectToAction("Index","Home");

        }

        [Authorize(Roles ="Admin")]
        public IActionResult SignOutAdmin()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

      


    }
}
