using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketProject.Models.Context;
using TicketProject.Models.Entities;

namespace TicketProject.Areas.Customer.Controllers { 

    [Authorize(Roles ="Customer")]
    [Area("Customer")]
    public class TicketController : Controller
    {
        private readonly TicketContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public TicketController(TicketContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            List<int> ticketnumbers = new();
            ticketnumbers.AddRange(Enumerable.Range(1, 200));
            ticketnumbers.FirstOrDefault();
            return View();
        }


        public IActionResult CreateTicket()
        {


            ViewBag.Category = _context.Categories;

            return View();
        }
        
        [HttpPost]
        public IActionResult CreateTicket(Ticket ticket)
        {
            
            if (ModelState.IsValid)
            {
                Random random = new();

                int randomnumber = random.Next(0, 20000000);

                ticket.ticketNumber = randomnumber; 

                while (_context.Tickets.Where(i => i.ticketNumber == randomnumber).FirstOrDefault() != null)
                {

                    randomnumber = random.Next(0, 20000000);
                    ticket.ticketNumber = randomnumber;

                }
                _context.Tickets.Add(ticket);

                var user = _context.Users.Where(i => i.UserName == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();

                ticket.AppUserId = user.Id;

                _context.SaveChanges();


                return RedirectToAction("Index", "Home", new { ticketnumber = ticket.ticketNumber });
            }
            ViewBag.Category = _context.Categories;
    

            return View(ticket);
        }

        public IActionResult TicketList()
        {

            var user = _context.Users.Where(i => i.UserName == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();

            var ticketlist = _context.Tickets.Include(i => i.Category)
                .Include(i => i.TicketStatus)
                .Where(i=>i.AppUserId == user.Id)
                .ToList();


            return View(ticketlist);

        }

        public IActionResult TicketFind()
        {
            return View();
        }


        [HttpPost]
        public IActionResult TicketFind(int ticketNumber)
        {



            var ticket = _context.Tickets.Where(i => i.ticketNumber == ticketNumber).FirstOrDefault();

            return RedirectToAction("TicketFindList", ticket);
        }

        public IActionResult TicketFindList()
        {



            var user = _context.Users.Where(i => i.UserName == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();

            var alltickets = _context.Tickets
                .Include(i=>i.Category)
                .Include(i=>i.TicketStatus)
                .Where(i => i.AppUserId == user.Id)
                .ToList();



            return View(alltickets);

        }

        [HttpPost]
        public IActionResult TicketFindList(int? ticketNumber)
        {

            if (ticketNumber == null)
            {
                return RedirectToAction("TicketFindList");

            }


            var user = _context.Users.Where(i => i.UserName == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();

            var repotscategorynames = _context.Tickets.Include(i => i.Category)
                .Include(i => i.TicketStatus)
                .Where(i => (i.ticketNumber == ticketNumber) && (i.AppUserId == user.Id))
                .ToList();



            var ticketdetail = _context.Tickets.Where(i => i.ticketNumber == ticketNumber).FirstOrDefault();

            ViewBag.Notes = _context.Notes.Where(i => i.TicketId == ticketdetail.Id);


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

            var userinfo = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

            note.AppUserId = userinfo.Id;

            note.IsEmployee = false;

            note.noteContent = textnote;

            await _context.Notes.AddAsync(note);

            _context.SaveChangesAsync();

            return RedirectToAction("TicketDetail", new { Id });

        }




    }
}
