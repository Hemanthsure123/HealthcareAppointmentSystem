using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFhirIdToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FhirId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FhirId",
                table: "Appointments");
        }
    }
}
