using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dbfunctionforqueuepositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               """
                CREATE OR REPLACE FUNCTION "GetBranchSlotsPositions"(branch_id INT)
                RETURNS TABLE(
                    "FullName" VARCHAR(200),
                    "StartAt" TIMESTAMPTZ,
                    "EndAt" TIMESTAMPTZ,
                    "Service" VARCHAR(200),
                    "RankPosition" BIGINT
                ) AS $$
                DECLARE
                    today TIMESTAMPTZ;
                    tomorrow TIMESTAMPTZ;
                BEGIN
                    today := date_trunc('day', now());
                    tomorrow := today + interval '1 day';

                    RETURN QUERY
                    WITH slots AS (
                        SELECT
                            ap."SlotId",
                            c."FullName",
                            s."StartAt",
                            s."EndAt",
                            st."Name",
                            ap."Id" AS "AppointmentId"
                        FROM "slots" s
                        LEFT JOIN "appointments" ap ON ap."SlotId" = s."Id"
                        LEFT JOIN "customers" c ON ap."CustomerId" = c."Id"
                        LEFT JOIN "service_types" st ON st."Id" = ap."ServiceTypeId"
                        WHERE s."BranchId" = branch_id
                          AND s."StartAt" >= today
                          AND s."EndAt" < tomorrow 
                    ),
                    numbered AS (
                        SELECT
                            "SlotId",
                            ROW_NUMBER() OVER (ORDER BY slots."EndAt") AS position
                        FROM slots
                        WHERE "AppointmentId" IS NOT NULL
                    )
                    SELECT
                        b."FullName",
                        b."StartAt",
                        b."EndAt",
                        b."Name" AS "Service",
                        n.position AS "RankPosition"
                    FROM slots b
                    LEFT JOIN numbered n ON b."SlotId" = n."SlotId"
                    ORDER BY b."EndAt";
                END;
                $$ LANGUAGE plpgsql;
                """);

            // Insert AuditActionType enum values
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (1, 'BookAppointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (2, 'CancelAppointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (3, 'RescheduleAppointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (4, 'UpdateAppointmentStatus') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (5, 'CreateSlot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (6, 'UpdateSlot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (7, 'SoftDeleteSlot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (8, 'AssignStaffService') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (9, 'AssignStaffBranch') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (10, 'HardDeleteExpiredSlots') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (11, 'CreateBranch') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO action_types (\"Id\", \"Type\") VALUES (12, 'UpdateBranch') ON CONFLICT DO NOTHING;");

            // Insert AuditEntityType enum values
            migrationBuilder.Sql("INSERT INTO entity_types (\"Id\", \"Type\") VALUES (1, 'Appointment') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO entity_types (\"Id\", \"Type\") VALUES (2, 'Slot') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO entity_types (\"Id\", \"Type\") VALUES (3, 'Staff') ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO entity_types (\"Id\", \"Type\") VALUES (4, 'Branch') ON CONFLICT DO NOTHING;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS ""GetBranchSlotsPositions""(INT);");
            migrationBuilder.Sql("DELETE FROM action_types WHERE \"id\" BETWEEN 1 AND 12;");
            migrationBuilder.Sql("DELETE FROM entity_types WHERE \"id\" BETWEEN 1 AND 4;");
        }
    }
}
