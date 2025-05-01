using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateSystem.Migrations
{
    /// <inheritdoc />
    public partial class Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KataId",
                table: "TourCompetitors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KataId",
                table: "TourCompetitors",
                type: "int",
                nullable: true);
        }
    }
}
