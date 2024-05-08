using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedHotelsTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Hotels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Hotels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_AppUserId",
                table: "Hotels",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_AspNetUsers_AppUserId",
                table: "Hotels",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_AspNetUsers_AppUserId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_AppUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
