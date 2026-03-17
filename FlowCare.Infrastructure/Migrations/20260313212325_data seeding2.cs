using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dataseeding2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEwlIFq98JIvuOWXxgd1RdyxOeeM81HjetAqbOsAM7Ei/9H0oHiXOdAlqLJW/aB6IA==");

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOnmfHxfWVap/2uYSWl3twuf4Nh7EeiB16aYmHydg4cvqBWlI7T0CnHk0sa3Rd57tg==");

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI60r6EGlKYDdSvqaf7T39KLm7EkQa025XrXlRNGoPzjcaZ5bKTSQ7upAh5WL6S4UQ==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHrF4Z8J/JAwn2MGdPNDxp+Mo/skpw3kpFtg3ZvtrS6QWxzRWbgrFmnrvYiMzyeFjg==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGsbs8r+F76VH2muR3fC/3vN5WdU2BTExix+GLUq8404G6J6v6DzqUNVUeuqIOKRaw==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELSPG5h+7BdPcPylkOW1nYtmegoLr8rHVswyeFDtv8u5TSLJ4kU/dBFJwiJdEMHJ2g==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENXh4ccYxR5ZiImvHV7GoFDRl8M6LDUSR+482USnGFs5tpSBqD/ur1iUYe4+S1ov4A==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBPLdZquGx5fqA/1o24IqRIrYZ+d3e4oWJ9m7rm4C7Uhf75nkFR/vqWhUwovQJbrrQ==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJd0IBZkPDKs8xz12g6ex/fUvMWOX6iUjd3Rc9YjZGlsXt0rGJxy0RFzuUJHamVy6A==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGU6g8vEh+QfJXTMTNSM0fj9XiJu4ejqvzWVWtJXnDY6n36CjAu1pj4+xeAqDwHKeA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOfn1uX/3oUCt3hcVLPLFdCP1Zd5XjUKIvXGYq3hRoQZmXXUn+XZpLFCfkECprifyQ==");

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB3n4s8ogXr0nisk5WHlpMdKlfqqcfyKb9wAXP6Qlicv3rISUagNTgTcpJccKZ/3BA==");

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI25+4Cf8C8Gnasac1q2lNeX6Xj7kYUU3Ey2brxZyS/Ox07YDMxBMwsQBoUwYavfJA==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC8xDeNpz9E8nc11DDSd4dcD9PVwadnRFJxfk8nfk7FqLUiuopHjmBJ44hGdz954jQ==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEICab8+wTwJozhRWUwreIBIMBkh3s2MHVKM1Yzp69eLAmZyvBMmtsK5ZSg9b+9NGJw==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEES+cw79Pa3WRk3nd1PVkOcv7ZVXJ5KJGfuSi4QNncMFCxFXTyUynYUAvukbqtsmkQ==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEzqb3dbU8+IioqpTJ0sycCRu3H90O6n7sG3DZYMc2woBJy+ycq94hESNK4mb0YAsg==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIHjdTv2h1anEJbgnEH/C408rfryyQWwwWpYzEz0pxy+bjhrKjj+Zk3FRm5rEV8pOw==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBOMdOXpbpPdJwdkQSW7KnPbBtwDrXTZ6zSCmD2p9VwsSHVG9xZcIDOaEJWLqs4L0w==");

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEfgZzmxWk+UqM6tXA+gt+wXwIuYwLSRO1EAbldfe1nZuwFVYVKBIQlkZD9AedT00w==");
        }
    }
}
