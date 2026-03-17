using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingRetentionPeriodforsoftdeletedrecordsofthebranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdRefImage",
                table: "customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetentionPerioud",
                table: "branches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AttachementReference",
                table: "appointments",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "appointments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AttachementReference",
                value: null);

            migrationBuilder.UpdateData(
                table: "appointments",
                keyColumn: "Id",
                keyValue: 2,
                column: "AttachementReference",
                value: null);

            migrationBuilder.UpdateData(
                table: "branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "RetentionPerioud",
                value: 30);

            migrationBuilder.UpdateData(
                table: "branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "RetentionPerioud",
                value: 30);

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "IdRefImage",
                value: null);

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "IdRefImage",
                value: null);

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "IdRefImage",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdRefImage",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "RetentionPerioud",
                table: "branches");

            migrationBuilder.DropColumn(
                name: "AttachementReference",
                table: "appointments");
        }
    }
}
