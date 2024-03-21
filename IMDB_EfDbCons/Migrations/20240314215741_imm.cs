using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB_EfDbCons.Migrations
{
    /// <inheritdoc />
    public partial class imm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreType = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreType);
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
                name: "TitleTypes",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleTypes", x => x.Type);
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

            migrationBuilder.CreateTable(
                name: "MovieBases",
                columns: table => new
                {
                    Tconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TitleTypeType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrimaryTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<DateOnly>(type: "date", nullable: true),
                    EndYear = table.Column<DateOnly>(type: "date", nullable: true),
                    RuntimeMins = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieBases", x => x.Tconst);
                    table.ForeignKey(
                        name: "FK_MovieBases_TitleTypes_TitleTypeType",
                        column: x => x.TitleTypeType,
                        principalTable: "TitleTypes",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    Tconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nconst = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => new { x.Tconst, x.Nconst });
                    table.ForeignKey(
                        name: "FK_Directors_MovieBases_Tconst",
                        column: x => x.Tconst,
                        principalTable: "MovieBases",
                        principalColumn: "Tconst",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Directors_Persons_Nconst",
                        column: x => x.Nconst,
                        principalTable: "Persons",
                        principalColumn: "Nconst",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    Tconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GenreType = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.Tconst, x.GenreType });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Genres_GenreType",
                        column: x => x.GenreType,
                        principalTable: "Genres",
                        principalColumn: "GenreType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenres_MovieBases_Tconst",
                        column: x => x.Tconst,
                        principalTable: "MovieBases",
                        principalColumn: "Tconst",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Writers",
                columns: table => new
                {
                    Tconst = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nconst = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writers", x => new { x.Tconst, x.Nconst });
                    table.ForeignKey(
                        name: "FK_Writers_MovieBases_Tconst",
                        column: x => x.Tconst,
                        principalTable: "MovieBases",
                        principalColumn: "Tconst",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Writers_Persons_Nconst",
                        column: x => x.Nconst,
                        principalTable: "Persons",
                        principalColumn: "Nconst",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Directors_Nconst",
                table: "Directors",
                column: "Nconst");

            migrationBuilder.CreateIndex(
                name: "IX_MovieBases_TitleTypeType",
                table: "MovieBases",
                column: "TitleTypeType");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreType",
                table: "MovieGenres",
                column: "GenreType");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalBlockbusters_Tconst",
                table: "PersonalBlockbusters",
                column: "Tconst");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCareers_PrimProf",
                table: "PersonalCareers",
                column: "PrimProf");

            migrationBuilder.CreateIndex(
                name: "IX_Writers_Nconst",
                table: "Writers",
                column: "Nconst");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "PersonalBlockbusters");

            migrationBuilder.DropTable(
                name: "PersonalCareers");

            migrationBuilder.DropTable(
                name: "Writers");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "MovieBases");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "TitleTypes");
        }
    }
}
