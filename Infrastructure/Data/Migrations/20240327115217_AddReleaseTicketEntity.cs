using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReleaseTicketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReleaseTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketTypeId = table.Column<int>(type: "int", nullable: false, comment: "票券所屬種類，關聯TicketTypes"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "票券所屬活動，關聯Events"),
                    MemberId = table.Column<int>(type: "int", nullable: false, comment: "票券持有者，關聯Members"),
                    ReleaseTicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "票券編號"),
                    OrderDetailId = table.Column<int>(type: "int", nullable: false, comment: "票券所屬訂單，關聯Orders"),
                    ParticipantName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "參加人姓名"),
                    ParticipantEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false, comment: "參加人Email"),
                    ParticipanPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "參加人電話"),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, comment: "票券異動狀態，"),
                    ExpireTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "到期時間"),
                    ChangedTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "異動時間，可空值"),
                    Staff = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "核銷人員")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseTickets_Events",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReleaseTickets_Members",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReleaseTickets_OrderDetails",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReleaseTickets_TicketTypes",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTickets_EventId",
                table: "ReleaseTickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTickets_MemberId",
                table: "ReleaseTickets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTickets_OrderDetailId",
                table: "ReleaseTickets",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTickets_TicketTypeId",
                table: "ReleaseTickets",
                column: "TicketTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReleaseTickets");

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
        }
    }
}
