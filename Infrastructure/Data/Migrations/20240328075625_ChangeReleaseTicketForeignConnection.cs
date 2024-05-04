using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReleaseTicketForeignConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseTickets_OrderDetails",
                table: "ReleaseTickets");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "ReleaseTickets",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReleaseTickets_OrderDetailId",
                table: "ReleaseTickets",
                newName: "IX_ReleaseTickets_OrderId");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8200), new DateTime(2024, 4, 30, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8193), new DateTime(2024, 4, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8187) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8206), new DateTime(2024, 5, 31, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8204), new DateTime(2024, 5, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8203) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8022));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8256), new DateTime(2024, 4, 27, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8261), new DateTime(2024, 3, 29, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 28, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8263), new DateTime(2024, 4, 27, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8265), new DateTime(2024, 3, 29, 15, 56, 25, 574, DateTimeKind.Local).AddTicks(8265) });

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseTickets_Orders",
                table: "ReleaseTickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseTickets_Orders",
                table: "ReleaseTickets");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ReleaseTickets",
                newName: "OrderDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ReleaseTickets_OrderId",
                table: "ReleaseTickets",
                newName: "IX_ReleaseTickets_OrderDetailId");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3931), new DateTime(2024, 4, 29, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3923), new DateTime(2024, 4, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3917) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3938), new DateTime(2024, 5, 30, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3936), new DateTime(2024, 5, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3935) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3660));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3680));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3987), new DateTime(2024, 4, 26, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3993), new DateTime(2024, 3, 28, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3992) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 27, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3996), new DateTime(2024, 4, 26, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3998), new DateTime(2024, 3, 28, 19, 52, 17, 682, DateTimeKind.Local).AddTicks(3997) });

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseTickets_OrderDetails",
                table: "ReleaseTickets",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
