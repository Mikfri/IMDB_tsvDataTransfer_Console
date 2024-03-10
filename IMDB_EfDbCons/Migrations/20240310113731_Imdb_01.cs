using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB_EfDbCons.Migrations
{
    /// <inheritdoc />
    public partial class Imdb_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieBases",
                columns: table => new
                {
                    Tconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TitleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: true),
                    EndYear = table.Column<int>(type: "int", nullable: true),
                    RuntimeMins = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieBases", x => x.Tconst);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Nconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrimaryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthYear = table.Column<DateOnly>(type: "date", nullable: true),
                    DeathYear = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Nconst);
                });

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    PrimaryProfession = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.PrimaryProfession);
                });

            migrationBuilder.CreateTable(
                name: "PersonalBlockbusters",
                columns: table => new
                {
                    Nconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tconst = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalBlockbusters", x => new { x.Nconst, x.Tconst });
                    table.ForeignKey(
                        name: "FK_PersonalBlockbusters_MovieBases_Tconst",
                        column: x => x.Tconst,
                        principalTable: "MovieBases",
                        principalColumn: "Tconst",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalBlockbusters_Persons_Nconst",
                        column: x => x.Nconst,
                        principalTable: "Persons",
                        principalColumn: "Nconst",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalCareers",
                columns: table => new
                {
                    Nconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrimProf = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCareers", x => new { x.Nconst, x.PrimProf });
                    table.ForeignKey(
                        name: "FK_PersonalCareers_Persons_Nconst",
                        column: x => x.Nconst,
                        principalTable: "Persons",
                        principalColumn: "Nconst",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalCareers_Professions_PrimProf",
                        column: x => x.PrimProf,
                        principalTable: "Professions",
                        principalColumn: "PrimaryProfession",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBlockbusters_Tconst",
                table: "PersonalBlockbusters",
                column: "Tconst");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCareers_PrimProf",
                table: "PersonalCareers",
                column: "PrimProf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalBlockbusters");

            migrationBuilder.DropTable(
                name: "PersonalCareers");

            migrationBuilder.DropTable(
                name: "MovieBases");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Professions");
        }
    }
}
