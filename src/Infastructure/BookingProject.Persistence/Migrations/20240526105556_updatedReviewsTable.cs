using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedReviewsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "CustomerReviews",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "CustomerReviews");
        }
    }
}
