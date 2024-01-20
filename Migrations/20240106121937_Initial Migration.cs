using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacePodBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PodcastSeries",
                columns: table => new
                {
                    PodcastSeriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Copyright = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Keywords = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodcastSeries", x => x.PodcastSeriesId);
                });

            migrationBuilder.CreateTable(
                name: "PodcastCategories",
                columns: table => new
                {
                    PodcastSeriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_PodcastCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PodcastCategories_PodcastSeries_PodcastSeriesId",
                        column: x => x.PodcastSeriesId,
                        principalTable: "PodcastSeries",
                        principalColumn: "PodcastSeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PodcastEpisodes",
                columns: table => new
                {
                    PodcastEpisodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    EpisodeDescription = table.Column<string>(type: "TEXT", nullable: true),
                    AudioSource = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    AudioType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    PodcastSeriesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodcastEpisodes", x => x.PodcastEpisodeId);
                    table.ForeignKey(
                        name: "FK_PodcastEpisodes_PodcastSeries_PodcastSeriesId",
                        column: x => x.PodcastSeriesId,
                        principalTable: "PodcastSeries",
                        principalColumn: "PodcastSeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PodcastCategories_CategoryId",
                table: "PodcastCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastCategories_PodcastSeriesId",
                table: "PodcastCategories",
                column: "PodcastSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastEpisodes_PodcastSeriesId",
                table: "PodcastEpisodes",
                column: "PodcastSeriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PodcastCategories");

            migrationBuilder.DropTable(
                name: "PodcastEpisodes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "PodcastSeries");
        }
    }
}
