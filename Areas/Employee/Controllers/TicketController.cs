using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketProject.Models.Context;
using TicketProject.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data.Odbc;

namespace TicketProject.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles ="Employee")]
    public class TicketController : Controller
    {

        private readonly TicketContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public TicketController(UserManager<AppUser> userManager, TicketContext context, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> TicketList()
        {


            var repotscategorynames = _context.Tickets.Include(i => i.Category)
                    .Include(i => i.TicketStatus)
                    .ToList();

            var userinfo = await _userManager.FindByNameAsync(_HttpContextAccessor.HttpContext.User.Identity.Name);
            ViewBag.User = userinfo;


            return View(repotscategorynames);

        }

        [HttpGet]
        public IActionResult TicketDetail(int Id)
        {

            var ticketdetail = _context.Tickets.Where(i => i.Id == Id).FirstOrDefault();

            ViewBag.Notes = _context.Notes.Where(i => i.TicketId == Id);

            return View(ticketdetail);

        }

        [HttpPost]
        public async Task<IActionResult> AddNote(int Id, string textnote)
        {
            Note note = new();

            note.TicketId = Id;

            var userinfo = await _userManager.FindByNameAsync(_HttpContextAccessor.HttpContext.User.Identity.Name);


            note.AppUserId = userinfo.Id;

            note.IsEmployee = true;

            note.noteContent = textnote;

            _context.Notes.Add(note);

            _context.SaveChanges();

            return RedirectToAction("TicketDetail",new {Id} );

        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int Id, int Id2)
        {

            var ticketdetail = _context.Tickets.Where(i => i.Id == Id).FirstOrDefault();
           
            if (ticketdetail.TicketStatusId < 3)
                ticketdetail.TicketStatusId = Id2 + 1;

            if (ticketdetail.TicketStatusId == 2)
            {
                var userinfo = await _userManager.FindByNameAsync(_HttpContextAccessor.HttpContext.User.Identity.Name);
                ticketdetail.AppUserIdEmployee = userinfo.Id;
              
            }



            _context.SaveChanges();

            return RedirectToAction("TicketDetail", new { Id });

        }

        






    }
}
