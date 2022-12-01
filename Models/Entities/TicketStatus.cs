namespace TicketProject.Models.Entities
{
    public class TicketStatus
    {
        public int Id { get; set; }

        public string? ticketStatusName { get; set; }

        public List<Ticket> Tickets { get; set; }

    }
}
