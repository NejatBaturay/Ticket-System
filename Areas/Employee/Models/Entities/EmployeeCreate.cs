using TicketProject.Models.Entities;

namespace TicketProject.Areas.Employee.Models.Entities
{
    public class EmployeeCreate
    {

        public int Id { get; set; }
        public string employeeName { get; set; }
        public string employeePassword { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
