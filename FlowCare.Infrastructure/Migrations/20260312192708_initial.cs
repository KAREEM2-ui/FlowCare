using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "action_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Timezone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "entity_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "branch_working_hours",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchId = table.Column<int>(type: "integer", maxLength: 64, nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch_working_hours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_branch_working_hours_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_types_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActorId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ActionTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MetadataJson = table.Column<string>(type: "jsonb", nullable: true),
                    ActionTypeId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audit_logs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_audit_logs_action_types_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "action_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_audit_logs_action_types_ActionTypeId1",
                        column: x => x.ActionTypeId1,
                        principalTable: "action_types",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_audit_logs_entity_types_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "entity_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_staff_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_staff_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "slots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "integer", nullable: false),
                    StaffId = table.Column<int>(type: "integer", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_slots_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_slots_service_types_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_slots_staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "staff_service_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StaffId = table.Column<int>(type: "integer", maxLength: 64, nullable: false),
                    ServiceTypeId = table.Column<int>(type: "integer", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_service_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_staff_service_types_service_types_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_staff_service_types_staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "staff_working_hours",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StaffId = table.Column<int>(type: "integer", maxLength: 64, nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_working_hours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_staff_working_hours_staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "integer", nullable: false),
                    SlotId = table.Column<int>(type: "integer", nullable: false),
                    StaffId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appointments_branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_service_types_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_action_types_Type",
                table: "action_types",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_BranchId_ServiceTypeId_CreatedAt",
                table: "appointments",
                columns: new[] { "BranchId", "ServiceTypeId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_CustomerId",
                table: "appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ServiceTypeId",
                table: "appointments",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_SlotId",
                table: "appointments",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_StaffId",
                table: "appointments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_ActionTypeId",
                table: "audit_logs",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_ActionTypeId1",
                table: "audit_logs",
                column: "ActionTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_EntityTypeId",
                table: "audit_logs",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_RoleId",
                table: "audit_logs",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_branch_working_hours_BranchId_DayOfWeek",
                table: "branch_working_hours",
                columns: new[] { "BranchId", "DayOfWeek" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entity_types_Type",
                table: "entity_types",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_types_BranchId",
                table: "service_types",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_slots_BranchId_ServiceTypeId_StaffId_StartAt",
                table: "slots",
                columns: new[] { "BranchId", "ServiceTypeId", "StaffId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_slots_ServiceTypeId",
                table: "slots",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_slots_StaffId",
                table: "slots",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_BranchId",
                table: "staff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_RoleId",
                table: "staff",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_service_types_ServiceTypeId",
                table: "staff_service_types",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_service_types_StaffId_ServiceTypeId",
                table: "staff_service_types",
                columns: new[] { "StaffId", "ServiceTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_working_hours_StaffId_DayOfWeek",
                table: "staff_working_hours",
                columns: new[] { "StaffId", "DayOfWeek" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "branch_working_hours");

            migrationBuilder.DropTable(
                name: "staff_service_types");

            migrationBuilder.DropTable(
                name: "staff_working_hours");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "slots");

            migrationBuilder.DropTable(
                name: "action_types");

            migrationBuilder.DropTable(
                name: "entity_types");

            migrationBuilder.DropTable(
                name: "service_types");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "branches");
        }
    }
}
