using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacePodBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    UserDataId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PodcastEpisodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.UserDataId);
                    table.ForeignKey(
                        name: "FK_UserData_PodcastEpisodes_PodcastEpisodeId",
                        column: x => x.PodcastEpisodeId,
                        principalTable: "PodcastEpisodes",
                        principalColumn: "PodcastEpisodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserData_PodcastEpisodeId",
                table: "UserData",
                column: "PodcastEpisodeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserData");
        }
    }
}
