﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedReservationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Reservations");
        }
    }
}
