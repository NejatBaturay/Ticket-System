using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketProject.Areas.Employee.Models.Entities;
using TicketProject.Models.Context;
using TicketProject.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TicketProject.Areas.Admin.Controllers
{

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public class CustomerController : Controller
        {

            private readonly TicketContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly RoleManager<AppRole> _roleManager;

            public CustomerController(UserManager<AppUser> userManager, TicketContext context, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
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


            public IActionResult CreateCustomer()
            {

               

                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CreateCustomer(CustomerCreate customer)
            {
            if (ModelState.IsValid)
            {
                AppUser customereuser = new()
                {
                    UserName = customer.customerUserName,
                    Name = customer.customerName,
                    Surname = customer.customerSurname,
                };

                var identityresult = await _userManager.CreateAsync(customereuser, customer.customerPassword);
                if (identityresult.Succeeded)
                {
                    if (!(_context.Roles.Any(i => i.Name == "Customer")))
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Customer"
                        });
                    }
                    await _userManager.AddToRoleAsync(customereuser, "Customer");
                    return RedirectToAction("Index", "Home");
                }
            }

                //ViewBag.Category = _context.Categories;

                return View(customer);

            }

            public IActionResult EmployeeList()
            {
                var employeeList = _context.Users.Include(i => i.Category).ToList();

                return View(employeeList);
            }



        }
    }

