using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateSystem.Migrations
{
    /// <inheritdoc />
    public partial class DodajRelacjeKategoriaStopien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KataCategories_Degrees_KataCatDegreeId",
                table: "KataCategories");

            migrationBuilder.DropIndex(
                name: "IX_KataCategories_KataCatDegreeId",
                table: "KataCategories");

            migrationBuilder.CreateTable(
                name: "CatKataDegrees",
                columns: table => new
                {
                    CatKataDegreeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KataCatId = table.Column<int>(type: "int", nullable: false),
                    DegreeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatKataDegrees", x => x.CatKataDegreeId);
                    table.ForeignKey(
                        name: "FK_CatKataDegrees_Degrees_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degrees",
                        principalColumn: "DegreeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatKataDegrees_KataCategories_KataCatId",
                        column: x => x.KataCatId,
                        principalTable: "KataCategories",
                        principalColumn: "KataCatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatKataDegrees_DegreeId",
                table: "CatKataDegrees",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CatKataDegrees_KataCatId",
                table: "CatKataDegrees",
                column: "KataCatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatKataDegrees");

            migrationBuilder.CreateIndex(
                name: "IX_KataCategories_KataCatDegreeId",
                table: "KataCategories",
                column: "KataCatDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_KataCategories_Degrees_KataCatDegreeId",
                table: "KataCategories",
                column: "KataCatDegreeId",
                principalTable: "Degrees",
                principalColumn: "DegreeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
