using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjustReleaseTicketsAndOrderTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseTickets_TicketTypes",
                table: "ReleaseTickets");

            migrationBuilder.DropIndex(
                name: "IX_ReleaseTickets_TicketTypeId",
                table: "ReleaseTickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketTypeId",
                table: "OrderTickets",
                type: "int",
                nullable: true,
                comment: "原票種的Id");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 4, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3232), new DateTime(2024, 5, 3, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3216), new DateTime(2024, 5, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3208) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 4, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3237), new DateTime(2024, 6, 4, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3235), new DateTime(2024, 6, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3235) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 4, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(2759));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 4, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(2780));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 4, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3283), new DateTime(2024, 5, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3288), new DateTime(2024, 4, 2, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3287) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 4, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3291), new DateTime(2024, 5, 1, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3293), new DateTime(2024, 4, 2, 17, 43, 29, 899, DateTimeKind.Local).AddTicks(3293) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketTypeId",
                table: "OrderTickets");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4442), new DateTime(2024, 5, 1, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4435), new DateTime(2024, 4, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4429) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4447), new DateTime(2024, 6, 1, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4445), new DateTime(2024, 5, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4444) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4221));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4236));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4529), new DateTime(2024, 4, 28, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4534), new DateTime(2024, 3, 30, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4532) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 29, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4536), new DateTime(2024, 4, 28, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4538), new DateTime(2024, 3, 30, 21, 20, 23, 579, DateTimeKind.Local).AddTicks(4537) });

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTickets_TicketTypeId",
                table: "ReleaseTickets",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseTickets_TicketTypes",
                table: "ReleaseTickets",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id");
        }
    }
}
