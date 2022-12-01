using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketProject.Models.Context;
using TicketProject.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TicketProject.Areas.Models.Entities;
using System.Security.Claims;

namespace TicketProject.Areas.Employee.Controllers
{
    [Area("Employee")]
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

            ViewBag.signInDate = DateTime.Now.ToString("HH:mm");

            var currentUser = _context.Users.Where(i => i.UserName == _HttpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();

            var userTickets = _context.Tickets.Where(i => i.AppUserIdEmployee == currentUser.Id).ToList();

            var userTicketsFresh = _context.Tickets.Where(i => i.AppUserIdEmployee == null).ToList();

            var sendedtickets = userTicketsFresh.Where(i => i.TicketStatusId == 1).Count();
            var reviewtickets = userTickets.Where(i => i.TicketStatusId == 2).Count();
            var closedtickets = userTickets.Where(i => i.TicketStatusId == 3).Count();

            var statuscountList = new List<int>();
            statuscountList.Add(sendedtickets);
            statuscountList.Add(reviewtickets);
            statuscountList.Add(closedtickets);


            ViewBag.name = currentUser.Name;
            ViewBag.surName = currentUser.Surname;
            return View(statuscountList);





        }



        //public IActionResult Login()
        //{

        //    return View();

        //}

        //[HttpPost]
        //public IActionResult Login(Employee employee)
        //{

        //    

        //    var loginemployee = employee;

        //    StringBuilder sb = new StringBuilder();

        //    using (SHA256 hash = SHA256Managed.Create())
        //    {
        //        Encoding enc = Encoding.UTF8;
        //        Byte[] result = hash.ComputeHash(enc.GetBytes(loginemployee.employeePassword));
        //        foreach (byte b in result) { sb.Append(b.ToString("x2")); }
        //    }
        //    loginemployee.employeePassword = sb.ToString();

        //    if (_context.Employees.Where(i => i.employeeName == loginemployee.employeeName).FirstOrDefault() == null)
        //    {



        //    }

        //    _context.Employees.Add(employee);

        //    _context.SaveChanges();

        //    return RedirectToAction("Index", "Home");

        //}

    
            
            public IActionResult SignOutEmployee()
            {
                _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home", new {area=""});
            }


    }

}