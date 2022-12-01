using System.ComponentModel.DataAnnotations;

namespace TicketProject.Models.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? registirationNumber { get; set; }

        public int? ticketNumber { get; set; }

        public string? ticketerName { get; set; }

        public DateTime? Created { get; set; }

        public string? ticketDesc { get; set; }
       
        public int? CategoryId { get; set; }

        public int? TicketStatusId { get; set; }

        public Category? Category { get; set; }

        public TicketStatus? TicketStatus { get; set; }

        public int? AppUserId {get;set;}

        public int? AppUserIdEmployee { get; set; }

        public AppUser? AppUser { get; set; }

        public List<Note>? Notes { get; set; }


    }
}
