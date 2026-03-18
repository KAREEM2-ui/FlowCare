using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_appointments_BranchId_ServiceTypeId_CreatedAt",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_SlotId",
                table: "appointments");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_BranchId",
                table: "appointments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_SlotId",
                table: "appointments",
                column: "SlotId",
                unique: true);




            // Insert AuditActionType enum values
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (1, 'BookAppointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (2, 'CancelAppointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (3, 'RescheduleAppointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (4, 'UpdateAppointmentStatus') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (5, 'CreateSlot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (6, 'UpdateSlot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (7, 'SoftDeleteSlot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (8, 'AssignStaffService') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (9, 'AssignStaffBranch') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (10, 'HardDeleteExpiredSlots') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (11, 'CreateBranch') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"action_types\" (\"Id\", \"Type\") VALUES (12, 'UpdateBranch') ON CONFLICT DO NOTHING;");

            // Insert AuditEntityType enum values
            migrationBuilder.Sql("INSERT INTO \"entity_types\" (\"Id\", \"Type\") VALUES (1, 'Appointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"entity_types\" (\"Id\", \"Type\") VALUES (2, 'Slot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"entity_types\" (\"Id\", \"Type\") VALUES (3, 'Staff') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"entity_types\" (\"Id\", \"Type\") VALUES (4, 'Branch') ON CONFLICT DO NOTHING;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_appointments_BranchId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_SlotId",
                table: "appointments");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_BranchId_ServiceTypeId_CreatedAt",
                table: "appointments",
                columns: new[] { "BranchId", "ServiceTypeId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_SlotId",
                table: "appointments",
                column: "SlotId");
        }
    }
}
