using Microsoft.AspNetCore.Mvc;
using TicketProject.Models;
using System.Diagnostics;
using TicketProject.Models.Entities;
using Microsoft.EntityFrameworkCore;
using TicketProject.Models.Context;
using Microsoft.AspNetCore.Identity;

namespace TicketProject.Controllers
{
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

            List<Category> categories = new()
            {
                new Category {categoryName = "Donanım"},
                new Category {categoryName = "İnşaat"}
            };
            
            List<TicketStatus> ticketStatuses = new()
            {
                new TicketStatus {ticketStatusName = "Ticket Sended"},
                new TicketStatus {ticketStatusName = "Ticket is fixing by employee"},
                new TicketStatus {ticketStatusName = "Ticket solved and closed"}
            };
            

            if (!_context.Categories.Any())
            {
                foreach (var item in categories)
                    _context.Categories.Add(item);
            }

            
            if (!_context.TicketStatuses.Any())
            {
                    
                foreach (var item in ticketStatuses)
                    _context.TicketStatuses.Add(item);
            }
            



            _context.SaveChanges();

            

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignIn model)
        {
            if (ModelState.IsValid)
            {
                var SignInResult = await _signInManager.PasswordSignInAsync(model.userName, model.userPassword, false, true);
                var userinfo = await _userManager.FindByNameAsync(model.userName);
                var userRole = await _userManager.GetRolesAsync(userinfo);

                if (SignInResult.Succeeded)
                {


                    if (userRole.Contains("Employee"))
                    {
                        return RedirectToAction("Index", "Home", new {area="Employee"});
                    }
                    else if (userRole.Contains("Customer"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                   

                }
                else if (SignInResult.IsLockedOut)
                {

                }
                else if (SignInResult.IsNotAllowed)
                {

                }


            }

            ModelState.AddModelError("", "There is no such an Employee or Customer with this username or password");
            return View("Index",model);

        }



        public IActionResult Privacy()
        {
            return View();
        }

       


    }
}