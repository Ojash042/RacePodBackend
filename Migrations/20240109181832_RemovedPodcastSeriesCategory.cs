using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacePodBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPodcastSeriesCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PodcastCategories");

            migrationBuilder.CreateTable(
                name: "CategoryPodcastSeries",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PodcastSeriesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPodcastSeries", x => new { x.CategoryId, x.PodcastSeriesId });
                    table.ForeignKey(
                        name: "FK_CategoryPodcastSeries_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPodcastSeries_PodcastSeries_PodcastSeriesId",
                        column: x => x.PodcastSeriesId,
                        principalTable: "PodcastSeries",
                        principalColumn: "PodcastSeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPodcastSeries_PodcastSeriesId",
                table: "CategoryPodcastSeries",
                column: "PodcastSeriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryPodcastSeries");

            migrationBuilder.CreateTable(
                name: "PodcastCategories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PodcastSeriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PodcastSeriesCategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_PodcastCategories_CategoryId",
                table: "PodcastCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastCategories_PodcastSeriesId",
                table: "PodcastCategories",
                column: "PodcastSeriesId");
        }
    }
}
