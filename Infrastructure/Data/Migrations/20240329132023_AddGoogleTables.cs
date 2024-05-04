using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoogleLoginInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Google回傳Name"),
                    Gamil = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false, comment: "Google回傳Gmail"),
                    NameIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Google回傳NameIdentifier欄位"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "建立時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleLoginInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleMemberRelation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false, comment: "會員Id，關連Members"),
                    GoogleLoginInfoId = table.Column<int>(type: "int", nullable: false, comment: "Google紀錄表Id，關聯GoogleLoginInfo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleMemberRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoogleMemberRelation_GoogleLoginInfo",
                        column: x => x.GoogleLoginInfoId,
                        principalTable: "GoogleLoginInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoogleMemberRelation_Members",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

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
                name: "IX_GoogleMemberRelation_GoogleLoginInfoId",
                table: "GoogleMemberRelation",
                column: "GoogleLoginInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleMemberRelation_MemberId",
                table: "GoogleMemberRelation",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleMemberRelation");

            migrationBuilder.DropTable(
                name: "GoogleLoginInfo");

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
        }
    }
}
