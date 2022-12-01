using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TicketProject.Models.Context;
using TicketProject.Areas.Models.Entities;
using TicketProject.Models.Entities;
using TicketProject.Areas.Employee.Models.Entities;

namespace TicketProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {

        private readonly TicketContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public EmployeeController(UserManager<AppUser> userManager, TicketContext context, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }
  

        public IActionResult Index()
        {
            return View();
        }

     
        public IActionResult CreateEmployee()
        {

            ViewBag.Category = _context.Categories;

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeCreate employee)
        {

            AppUser employeeuser = new()
            {
                UserName = employee.employeeName,
                CategoryId = employee.CategoryId  
            };

            var identityresult = await _userManager.CreateAsync(employeeuser, employee.employeePassword);
            if (identityresult.Succeeded)
            {
                if (!(_context.Roles.Any(i => i.Name == "Employee")))
                {
                    await _roleManager.CreateAsync(new()
                    {
                        Name = "Employee"
                    });
                }
                await _userManager.AddToRoleAsync(employeeuser, "Employee");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Category = _context.Categories;

            return View();

        }

        public IActionResult EmployeeList()
        {
            var employeeList = _context.Users.Include(i=>i.Category).ToList();

            return View(employeeList);
        }

        public IActionResult EmployeeDelete(int id)
        {

            var employee = _context.Users.Find(id);



            _userManager.DeleteAsync(employee);




            return RedirectToAction("EmployeeList");

        }





    }
}
