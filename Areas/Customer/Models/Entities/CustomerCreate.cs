using System.ComponentModel.DataAnnotations;
namespace TicketProject.Areas.Employee.Models.Entities
{
    public class CustomerCreate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string customerUserName { get; set; }
        [Required]
        public string customerPassword { get; set; }
        [Required]
        public string customerName { get; set; }
        [Required]
        public string customerSurname { get; set; }
    }
}
