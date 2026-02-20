using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndex_CourseRegistration_Participant_Session : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseRegistrations_ParticipantId",
                table: "CourseRegistrations");

            migrationBuilder.CreateIndex(
                name: "UQ_CourseRegistration_Participant_Session",
                table: "CourseRegistrations",
                columns: new[] { "ParticipantId", "CourseSessionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_CourseRegistration_Participant_Session",
                table: "CourseRegistrations");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRegistrations_ParticipantId",
                table: "CourseRegistrations",
                column: "ParticipantId");
        }
    }
}
