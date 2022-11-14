using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Migrations
{
    public partial class UpdateMenteeBooking3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Mentee_MenteeId",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "PayPaymentRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Mentee_MenteeId",
                table: "Booking",
                column: "MenteeId",
                principalTable: "Mentee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Mentee_MenteeId",
                table: "Booking");

            migrationBuilder.CreateTable(
                name: "PayPaymentRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPaymentRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayPaymentRequests_CustomerId",
                table: "PayPaymentRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PayPaymentRequests_State",
                table: "PayPaymentRequests",
                column: "State");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Mentee_MenteeId",
                table: "Booking",
                column: "MenteeId",
                principalTable: "Mentee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
