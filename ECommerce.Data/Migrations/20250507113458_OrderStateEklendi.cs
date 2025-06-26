using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderStateEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderState",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 5, 7, 14, 34, 57, 749, DateTimeKind.Local).AddTicks(3935), new Guid("62d89a3e-a06f-4532-aa8c-a64021b78484") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 5, 7, 14, 34, 57, 751, DateTimeKind.Local).AddTicks(9600));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 5, 7, 14, 34, 57, 752, DateTimeKind.Local).AddTicks(415));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AppUsers_AppUserId",
                table: "Orders",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AppUsers_AppUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderState",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 15, 16, 43, 2, 658, DateTimeKind.Local).AddTicks(432), new Guid("40dca603-d213-4847-ac2c-566a2a61f8b4") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 4, 15, 16, 43, 2, 660, DateTimeKind.Local).AddTicks(3710));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 4, 15, 16, 43, 2, 660, DateTimeKind.Local).AddTicks(4555));
        }
    }
}
