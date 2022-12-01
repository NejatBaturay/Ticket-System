

namespace TicketProject.Models.Entities
{
    public class Note
    {
        public int Id { get; set; }

        public string noteContent { get; set; }

        public int TicketId { get; set; }

        public Ticket Ticket { get; set; }

        public int CustomerId { get; set; }

        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public bool IsEmployee { get; set; }


    }
}
