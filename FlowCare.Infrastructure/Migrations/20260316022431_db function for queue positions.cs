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
                    WITH booked AS (
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
                            ROW_NUMBER() OVER (ORDER BY "EndAt") AS position
                        FROM booked
                        WHERE "AppointmentId" IS NOT NULL
                    )
                    SELECT
                        b."FullName",
                        b."StartAt",
                        b."EndAt",
                        b."Name" AS "Service",
                        n.position AS "RankPosition"
                    FROM booked b
                    LEFT JOIN numbered n ON b."SlotId" = n."SlotId"
                    ORDER BY b."EndAt";
                END;
                $$ LANGUAGE plpgsql;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS ""GetBranchSlotsPositions""(INT);");
        }
    }
}
