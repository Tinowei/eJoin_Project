using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChanegeForeignConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartTickets_TicketTypes",
                table: "CartTickets");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9110), new DateTime(2024, 4, 24, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9097), new DateTime(2024, 4, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9090) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9114), new DateTime(2024, 5, 25, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9113), new DateTime(2024, 5, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9112) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(8867));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9153), new DateTime(2024, 4, 21, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9157), new DateTime(2024, 3, 23, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9156) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 22, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9160), new DateTime(2024, 4, 21, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9161), new DateTime(2024, 3, 23, 12, 30, 10, 292, DateTimeKind.Local).AddTicks(9161) });

            migrationBuilder.CreateIndex(
                name: "IX_CartTickets_TicketTypeId",
                table: "CartTickets",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartTickets_TicketTypes1",
                table: "CartTickets",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartTickets_TicketTypes1",
                table: "CartTickets");

            migrationBuilder.DropIndex(
                name: "IX_CartTickets_TicketTypeId",
                table: "CartTickets");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8692), new DateTime(2024, 4, 23, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8684), new DateTime(2024, 4, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8678) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8696), new DateTime(2024, 5, 24, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8695), new DateTime(2024, 5, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8694) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8443));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8456));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8740), new DateTime(2024, 4, 20, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8743), new DateTime(2024, 3, 22, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8743) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 21, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8746), new DateTime(2024, 4, 20, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8747), new DateTime(2024, 3, 22, 14, 55, 4, 243, DateTimeKind.Local).AddTicks(8746) });

            migrationBuilder.AddForeignKey(
                name: "FK_CartTickets_TicketTypes",
                table: "CartTickets",
                column: "CartId",
                principalTable: "TicketTypes",
                principalColumn: "Id");
        }
    }
}
