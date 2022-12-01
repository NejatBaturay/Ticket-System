using Microsoft.AspNetCore.Identity;

namespace TicketProject.Models.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Ticket> Tickets { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }


    }
}
