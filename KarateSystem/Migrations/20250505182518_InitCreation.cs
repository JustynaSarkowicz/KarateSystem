using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClubPlace = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    DegreeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.DegreeId);
                });

            migrationBuilder.CreateTable(
                name: "KataCategories",
                columns: table => new
                {
                    KataCatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KataCatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KataCatGender = table.Column<bool>(type: "bit", nullable: true),
                    KataCatAgeMin = table.Column<int>(type: "int", nullable: false),
                    KataCatAgeMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KataCategories", x => x.KataCatId);
                });

            migrationBuilder.CreateTable(
                name: "KumiteCategories",
                columns: table => new
                {
                    KumiteCatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KumiteCatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KumiteCatGender = table.Column<bool>(type: "bit", nullable: false),
                    KumiteCatAgeMin = table.Column<int>(type: "int", nullable: false),
                    KumiteCatAgeMax = table.Column<int>(type: "int", nullable: false),
                    KumiteCatWeightMin = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    KumiteCatWeightMax = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KumiteCategories", x => x.KumiteCatId);
                });

            migrationBuilder.CreateTable(
                name: "Mats",
                columns: table => new
                {
                    MatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mats", x => x.MatId);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourDate = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TourId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    CompId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompDateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    CompGender = table.Column<bool>(type: "bit", nullable: false),
                    CompWeight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CompDegreeId = table.Column<int>(type: "int", nullable: false),
                    CompClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.CompId);
                    table.ForeignKey(
                        name: "FK_Competitors_Clubs_CompClubId",
                        column: x => x.CompClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitors_Degrees_CompDegreeId",
                        column: x => x.CompDegreeId,
                        principalTable: "Degrees",
                        principalColumn: "DegreeId",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "TourCatKatas",
                columns: table => new
                {
                    TourCatKataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    KataCatId = table.Column<int>(type: "int", nullable: false),
                    MatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCatKatas", x => x.TourCatKataId);
                    table.ForeignKey(
                        name: "FK_TourCatKatas_KataCategories_KataCatId",
                        column: x => x.KataCatId,
                        principalTable: "KataCategories",
                        principalColumn: "KataCatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCatKatas_Mats_MatId",
                        column: x => x.MatId,
                        principalTable: "Mats",
                        principalColumn: "MatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCatKatas_Tournaments_TourId",
                        column: x => x.TourId,
                        principalTable: "Tournaments",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourCatKumites",
                columns: table => new
                {
                    TourCatKumiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    KumiteCatId = table.Column<int>(type: "int", nullable: false),
                    MatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCatKumites", x => x.TourCatKumiteId);
                    table.ForeignKey(
                        name: "FK_TourCatKumites_KumiteCategories_KumiteCatId",
                        column: x => x.KumiteCatId,
                        principalTable: "KumiteCategories",
                        principalColumn: "KumiteCatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCatKumites_Mats_MatId",
                        column: x => x.MatId,
                        principalTable: "Mats",
                        principalColumn: "MatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCatKumites_Tournaments_TourId",
                        column: x => x.TourId,
                        principalTable: "Tournaments",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourCompetitors",
                columns: table => new
                {
                    TourCompId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    CompId = table.Column<int>(type: "int", nullable: false),
                    TourCatKataId = table.Column<int>(type: "int", nullable: true),
                    TourCatKumiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCompetitors", x => x.TourCompId);
                    table.ForeignKey(
                        name: "FK_TourCompetitors_Competitors_CompId",
                        column: x => x.CompId,
                        principalTable: "Competitors",
                        principalColumn: "CompId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCompetitors_TourCatKatas_TourCatKataId",
                        column: x => x.TourCatKataId,
                        principalTable: "TourCatKatas",
                        principalColumn: "TourCatKataId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCompetitors_TourCatKumites_TourCatKumiteId",
                        column: x => x.TourCatKumiteId,
                        principalTable: "TourCatKumites",
                        principalColumn: "TourCatKumiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourCompetitors_Tournaments_TourId",
                        column: x => x.TourId,
                        principalTable: "Tournaments",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    FightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourCatKumiteId = table.Column<int>(type: "int", nullable: false),
                    RedCompetitorId = table.Column<int>(type: "int", nullable: true),
                    BlueCompetitorId = table.Column<int>(type: "int", nullable: true),
                    WinnerId = table.Column<int>(type: "int", nullable: true),
                    RedCompetitorScore = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    BlueCompetitorScore = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    FightTime = table.Column<int>(type: "int", nullable: true),
                    FightNumOverTime = table.Column<int>(type: "int", nullable: true),
                    FightWalkover = table.Column<bool>(type: "bit", nullable: false),
                    Round = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.FightId);
                    table.ForeignKey(
                        name: "FK_Fights_TourCatKumites_TourCatKumiteId",
                        column: x => x.TourCatKumiteId,
                        principalTable: "TourCatKumites",
                        principalColumn: "TourCatKumiteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fights_TourCompetitors_BlueCompetitorId",
                        column: x => x.BlueCompetitorId,
                        principalTable: "TourCompetitors",
                        principalColumn: "TourCompId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fights_TourCompetitors_RedCompetitorId",
                        column: x => x.RedCompetitorId,
                        principalTable: "TourCompetitors",
                        principalColumn: "TourCompId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fights_TourCompetitors_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "TourCompetitors",
                        principalColumn: "TourCompId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Katas",
                columns: table => new
                {
                    KataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KataRate1 = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    KataRate2 = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    KataRate3 = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    KataRate4 = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    KataRate5 = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    KataScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Overtime = table.Column<int>(type: "int", nullable: true),
                    TourCompId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Katas", x => x.KataId);
                    table.ForeignKey(
                        name: "FK_Katas_TourCompetitors_TourCompId",
                        column: x => x.TourCompId,
                        principalTable: "TourCompetitors",
                        principalColumn: "TourCompId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatKataDegrees_DegreeId",
                table: "CatKataDegrees",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CatKataDegrees_KataCatId",
                table: "CatKataDegrees",
                column: "KataCatId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompClubId",
                table: "Competitors",
                column: "CompClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompDegreeId",
                table: "Competitors",
                column: "CompDegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_BlueCompetitorId",
                table: "Fights",
                column: "BlueCompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_RedCompetitorId",
                table: "Fights",
                column: "RedCompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_TourCatKumiteId",
                table: "Fights",
                column: "TourCatKumiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_WinnerId",
                table: "Fights",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Katas_TourCompId",
                table: "Katas",
                column: "TourCompId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourCatKatas_KataCatId",
                table: "TourCatKatas",
                column: "KataCatId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCatKatas_MatId",
                table: "TourCatKatas",
                column: "MatId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCatKatas_TourId",
                table: "TourCatKatas",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCatKumites_KumiteCatId",
                table: "TourCatKumites",
                column: "KumiteCatId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCatKumites_MatId",
                table: "TourCatKumites",
                column: "MatId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCatKumites_TourId",
                table: "TourCatKumites",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCompetitors_CompId",
                table: "TourCompetitors",
                column: "CompId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCompetitors_TourCatKataId",
                table: "TourCompetitors",
                column: "TourCatKataId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCompetitors_TourCatKumiteId",
                table: "TourCompetitors",
                column: "TourCatKumiteId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCompetitors_TourId",
                table: "TourCompetitors",
                column: "TourId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatKataDegrees");

            migrationBuilder.DropTable(
                name: "Fights");

            migrationBuilder.DropTable(
                name: "Katas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TourCompetitors");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "TourCatKatas");

            migrationBuilder.DropTable(
                name: "TourCatKumites");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.DropTable(
                name: "KataCategories");

            migrationBuilder.DropTable(
                name: "KumiteCategories");

            migrationBuilder.DropTable(
                name: "Mats");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
