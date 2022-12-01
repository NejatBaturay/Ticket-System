using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketProject.Models.Migrations
{
    public partial class isEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmployee",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmployee",
                table: "Notes");
        }
    }
}
