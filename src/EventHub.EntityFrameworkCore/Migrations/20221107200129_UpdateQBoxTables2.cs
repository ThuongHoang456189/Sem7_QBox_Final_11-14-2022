using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Migrations
{
    public partial class UpdateQBoxTables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingId1",
                table: "Question",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId1",
                table: "MentorSkill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MentorSkillId1",
                table: "Certificate",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MenteeId1",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SlotId1",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_BookingId1",
                table: "Question",
                column: "BookingId1");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSkill_SubjectId1",
                table: "MentorSkill",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_MentorSkillId1",
                table: "Certificate",
                column: "MentorSkillId1");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_MenteeId1",
                table: "Booking",
                column: "MenteeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_SlotId1",
                table: "Booking",
                column: "SlotId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Mentee_MenteeId1",
                table: "Booking",
                column: "MenteeId1",
                principalTable: "Mentee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Slot_SlotId1",
                table: "Booking",
                column: "SlotId1",
                principalTable: "Slot",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_MentorSkill_MentorSkillId1",
                table: "Certificate",
                column: "MentorSkillId1",
                principalTable: "MentorSkill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MentorSkill_Subject_SubjectId1",
                table: "MentorSkill",
                column: "SubjectId1",
                principalTable: "Subject",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Booking_BookingId1",
                table: "Question",
                column: "BookingId1",
                principalTable: "Booking",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Mentee_MenteeId1",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Slot_SlotId1",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_MentorSkill_MentorSkillId1",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_MentorSkill_Subject_SubjectId1",
                table: "MentorSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Booking_BookingId1",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_BookingId1",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_MentorSkill_SubjectId1",
                table: "MentorSkill");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_MentorSkillId1",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Booking_MenteeId1",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_SlotId1",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "SubjectId1",
                table: "MentorSkill");

            migrationBuilder.DropColumn(
                name: "MentorSkillId1",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "MenteeId1",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "SlotId1",
                table: "Booking");
        }
    }
}
