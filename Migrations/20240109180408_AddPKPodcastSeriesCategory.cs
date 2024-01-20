using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacePodBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPKPodcastSeriesCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PodcastSeriesCategoryId",
                table: "PodcastCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PodcastSeriesCategoryId",
                table: "PodcastCategories");
        }
    }
}
