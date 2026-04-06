using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibreCare.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "Profiles",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Profiles",
                newName: "IsDefault");
        }
    }
}
