using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participants_EventId_Email",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Participants");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Participants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId_UserId",
                table: "Participants",
                columns: new[] { "EventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_EventId_UserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Participants");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Participants",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId_Email",
                table: "Participants",
                columns: new[] { "EventId", "Email" },
                unique: true);
        }
    }
}
