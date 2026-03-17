using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dataseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "BranchManager" },
                    { 3, "Staff" },
                    { 4, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "action_types",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "SEED_IMPORT" },
                    { 2, "APPOINTMENT_BOOKED" }
                });

            migrationBuilder.InsertData(
                table: "branches",
                columns: new[] { "Id", "Address", "City", "IsActive", "Name", "Timezone" },
                values: new object[,]
                {
                    { 1, "Al Khuwair, Muscat", "Muscat", true, "FlowCare Muscat - Al Khuwair", "Asia/Muscat" },
                    { 2, "Al Humbar, Suhar", "Suhar", true, "FlowCare Suhar - Al Humbar", "Asia/Muscat" }
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "Id", "Email", "FullName", "IsActive", "PasswordHash", "Phone", "Username" },
                values: new object[,]
                {
                    { 1, "ahmed.h@example.com", "Ahmed Al Harthy", true, "AQAAAAIAAYagAAAAEOfn1uX/3oUCt3hcVLPLFdCP1Zd5XjUKIvXGYq3hRoQZmXXUn+XZpLFCfkECprifyQ==", "+96890000001", "cust_ahmed" },
                    { 2, "fatima.z@example.com", "Fatima Al Zadjali", true, "AQAAAAIAAYagAAAAEB3n4s8ogXr0nisk5WHlpMdKlfqqcfyKb9wAXP6Qlicv3rISUagNTgTcpJccKZ/3BA==", "+96890000002", "cust_fatima" },
                    { 3, "khalid.a@example.com", "Khalid Al Amri", true, "AQAAAAIAAYagAAAAEI25+4Cf8C8Gnasac1q2lNeX6Xj7kYUU3Ey2brxZyS/Ox07YDMxBMwsQBoUwYavfJA==", "+96890000003", "cust_khalid" }
                });

            migrationBuilder.InsertData(
                table: "entity_types",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "SYSTEM" },
                    { 2, "APPOINTMENT" }
                });

            migrationBuilder.InsertData(
                table: "audit_logs",
                columns: new[] { "Id", "ActionTypeId", "ActionTypeId1", "ActorId", "EntityId", "EntityTypeId", "MetadataJson", "RoleId", "Timestamp" },
                values: new object[,]
                {
                    { 1, 1, null, 1, 1, 1, "{\"note\":\"Initial seed import executed\"}", 1, new DateTimeOffset(new DateTime(2026, 2, 15, 10, 10, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 2, 2, null, 1, 1, 2, "{\"slot_id\":1,\"branch_id\":1,\"service_type_id\":1}", 4, new DateTimeOffset(new DateTime(2026, 2, 15, 10, 12, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "service_types",
                columns: new[] { "Id", "BranchId", "Description", "DurationMinutes", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Account questions, basic requests, ticket follow-ups", 15, true, "Customer Support" },
                    { 2, 1, "Check documents, validate requirements, approvals", 20, true, "Document Verification" },
                    { 3, 1, "Priority desk for special cases", 30, true, "VIP Service Desk" },
                    { 4, 2, "Account questions, basic requests, ticket follow-ups", 15, true, "Customer Support" },
                    { 5, 2, "New customer onboarding and profile creation", 20, true, "New Registration" },
                    { 6, 2, "Escalations handling and complaints resolution", 25, true, "Complaints & Escalations" }
                });

            migrationBuilder.InsertData(
                table: "staff",
                columns: new[] { "Id", "BranchId", "Email", "FullName", "IsActive", "PasswordHash", "RoleId", "Username" },
                values: new object[,]
                {
                    { 1, 1, "admin@flowcare.local", "System Admin", true, "AQAAAAIAAYagAAAAEC8xDeNpz9E8nc11DDSd4dcD9PVwadnRFJxfk8nfk7FqLUiuopHjmBJ44hGdz954jQ==", 1, "admin" },
                    { 2, 1, "aisha.b@flowcare.local", "Aisha Al Balushi", true, "AQAAAAIAAYagAAAAEICab8+wTwJozhRWUwreIBIMBkh3s2MHVKM1Yzp69eLAmZyvBMmtsK5ZSg9b+9NGJw==", 2, "mgr_muscat" },
                    { 3, 2, "hamad.h@flowcare.local", "Hamad Al Hinai", true, "AQAAAAIAAYagAAAAEES+cw79Pa3WRk3nd1PVkOcv7ZVXJ5KJGfuSi4QNncMFCxFXTyUynYUAvukbqtsmkQ==", 2, "mgr_suhar" },
                    { 4, 1, "salim.r@flowcare.local", "Salim Al Rashdi", true, "AQAAAAIAAYagAAAAEEzqb3dbU8+IioqpTJ0sycCRu3H90O6n7sG3DZYMc2woBJy+ycq94hESNK4mb0YAsg==", 3, "staff_muscat_1" },
                    { 5, 1, "maha.s@flowcare.local", "Maha Al Shukaili", true, "AQAAAAIAAYagAAAAEIHjdTv2h1anEJbgnEH/C408rfryyQWwwWpYzEz0pxy+bjhrKjj+Zk3FRm5rEV8pOw==", 3, "staff_muscat_2" },
                    { 6, 2, "nasser.m@flowcare.local", "Nasser Al Maqbali", true, "AQAAAAIAAYagAAAAEBOMdOXpbpPdJwdkQSW7KnPbBtwDrXTZ6zSCmD2p9VwsSHVG9xZcIDOaEJWLqs4L0w==", 3, "staff_suhar_1" },
                    { 7, 2, "reem.k@flowcare.local", "Reem Al Kindi", true, "AQAAAAIAAYagAAAAEEfgZzmxWk+UqM6tXA+gt+wXwIuYwLSRO1EAbldfe1nZuwFVYVKBIQlkZD9AedT00w==", 3, "staff_suhar_2" }
                });

            migrationBuilder.InsertData(
                table: "slots",
                columns: new[] { "Id", "BranchId", "Capacity", "DeletedAt", "EndAt", "IsActive", "ServiceTypeId", "StaffId", "StartAt" },
                values: new object[,]
                {
                    { 1, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 1, 4, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 2, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 1, 4, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 3, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 17, 10, 20, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 2, 4, new DateTimeOffset(new DateTime(2026, 2, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 4, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 18, 11, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 3, 5, new DateTimeOffset(new DateTime(2026, 2, 18, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 5, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 19, 12, 20, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 2, 4, new DateTimeOffset(new DateTime(2026, 2, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 6, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 3, 5, new DateTimeOffset(new DateTime(2026, 2, 20, 9, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 7, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 4, 6, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 8, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 50, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 5, 6, new DateTimeOffset(new DateTime(2026, 2, 16, 9, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 9, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 17, 10, 25, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 6, 7, new DateTimeOffset(new DateTime(2026, 2, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 10, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 18, 10, 55, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 6, 7, new DateTimeOffset(new DateTime(2026, 2, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 11, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 19, 11, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 4, 6, new DateTimeOffset(new DateTime(2026, 2, 19, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 12, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 20, 12, 20, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 5, 6, new DateTimeOffset(new DateTime(2026, 2, 20, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 13, 1, 1, null, new DateTimeOffset(new DateTime(2026, 2, 21, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 1, 4, new DateTimeOffset(new DateTime(2026, 2, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 14, 2, 1, null, new DateTimeOffset(new DateTime(2026, 2, 22, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), true, 4, 6, new DateTimeOffset(new DateTime(2026, 2, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "staff_service_types",
                columns: new[] { "Id", "ServiceTypeId", "StaffId" },
                values: new object[,]
                {
                    { 1, 1, 4 },
                    { 2, 2, 4 },
                    { 3, 3, 5 },
                    { 4, 4, 6 },
                    { 5, 5, 6 },
                    { 6, 6, 7 }
                });

            migrationBuilder.InsertData(
                table: "appointments",
                columns: new[] { "Id", "BranchId", "CreatedAt", "CustomerId", "ServiceTypeId", "SlotId", "StaffId", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTimeOffset(new DateTime(2026, 2, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), 1, 1, 1, 4, 1 },
                    { 2, 2, new DateTimeOffset(new DateTime(2026, 2, 15, 10, 5, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), 2, 6, 9, 7, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "audit_logs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "audit_logs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "staff_service_types",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "staff_service_types",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "staff_service_types",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "staff_service_types",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "staff_service_types",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "staff_service_types",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "action_types",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "action_types",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "entity_types",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "entity_types",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "slots",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "branches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "branches",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
