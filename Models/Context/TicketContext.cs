using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketProject.Models;
using TicketProject.Models.Entities;
using TicketProject.Areas.Models.Entities;
using TicketProject.Areas.Employee.Models.Entities;

namespace TicketProject.Models.Context
{
    public class TicketContext: IdentityDbContext<AppUser,AppRole,int>
    {
        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Category> Categories { get; set; }

     
        public DbSet<TicketStatus> TicketStatuses { get; set; }
    

        public DbSet<Note> Notes { get; set; }

        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {

        }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ticket>().HasOne(i => i.Category).WithMany(i => i.Tickets).HasForeignKey(i => i.CategoryId);
            modelBuilder.Entity<EmployeeCreate>().HasOne(i => i.Category).WithMany(i => i.Employees).HasForeignKey(i => i.CategoryId);
            modelBuilder.Entity<Ticket>().HasOne(i => i.TicketStatus).WithMany(i => i.Tickets).HasForeignKey(i => i.TicketStatusId);
            modelBuilder.Entity<Ticket>().Property(i => i.Created).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Ticket>().Property(i => i.TicketStatusId).HasDefaultValueSql("1");
            base.OnModelCreating(modelBuilder);
        }
    }
}
