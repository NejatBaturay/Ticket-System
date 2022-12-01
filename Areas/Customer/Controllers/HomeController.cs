using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TicketProject.Models.Context;
using TicketProject.Models.Entities;

namespace TicketProject.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("Customer")]
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


        public IActionResult Index(int? ticketnumber)
        {
            ViewBag.signInDate = DateTime.Now.ToString("HH:mm");

            var currentUser =  _context.Users.Where(i => i.UserName == _HttpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();

            var userTickets = _context.Tickets.Where(i=>i.AppUserId==currentUser.Id).ToList();

            var sendedtickets = userTickets.Where(i => i.TicketStatusId == 1).Count();
            var reviewtickets = userTickets.Where(i => i.TicketStatusId == 2).Count();
            var closedtickets = userTickets.Where(i => i.TicketStatusId == 3).Count();

            var statuscountList = new List<int>();
            statuscountList.Add(sendedtickets);
            statuscountList.Add(reviewtickets);
            statuscountList.Add(closedtickets);


            ViewBag.name = currentUser.Name;
            ViewBag.surName = currentUser.Surname;
            ViewBag.ticketnumber = ticketnumber;
            return View(statuscountList);
        }

        public IActionResult SignOutCustomer()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new{ area = ""});
        }

    }
}
