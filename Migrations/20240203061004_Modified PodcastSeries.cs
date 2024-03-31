using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacePodBackend.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedPodcastSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RssUrl",
                table: "PodcastSeries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RssUrl",
                table: "PodcastSeries");
        }
    }
}
