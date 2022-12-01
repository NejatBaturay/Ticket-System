using TicketProject.Areas.Employee.Models.Entities;

namespace TicketProject.Models.Entities
{
    public class Category
    {

        public int Id { get; set; }

        public string categoryName  { get; set; }

        public List<EmployeeCreate> Employees { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
