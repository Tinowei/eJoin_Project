using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewOrdersAndCartsAndECPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false, comment: "購物車所屬會員，關聯Members"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "購物車建立時間"),
                    ExpiredTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "購物車到期時間"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "購物車對應活動，關聯Events"),
                    ParticipantName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "參加人姓名"),
                    ParticipantEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true, comment: "參加人Email"),
                    ParticipantPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "參加人電話"),
                    LearnedFrom = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "活動資訊來源")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Events",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Carts_Members",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ECPayLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelateOrderId = table.Column<int>(type: "int", nullable: false, comment: "對應到的OrderId"),
                    MerchantTradeNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "傳送給綠界，特店交易編號"),
                    TotalAmount = table.Column<int>(type: "int", nullable: false, comment: "傳送給綠界，交易金額"),
                    ItemName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, comment: "傳送給綠界，商品名稱"),
                    MerchantTradeDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "傳送給綠界，特店交易時間"),
                    CheckMacValue = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "傳送給綠界，檢查碼"),
                    RtnCode = table.Column<int>(type: "int", nullable: true, comment: "綠界回傳值，交易狀態"),
                    RtnMsg = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "綠界回傳值，交易訊息"),
                    TradeNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "綠界回傳值，綠界交易編號"),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "綠界回傳值，付款時間"),
                    TradeDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "綠界回傳值，訂單成立時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECPayLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "訂單編號"),
                    BuyerId = table.Column<int>(type: "int", nullable: false, comment: "買家Id，關聯Members"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "活動Id，關聯Events"),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, comment: "訂單狀態，1為待付款，2為已付款"),
                    ExpiredTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "待付款的到期時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Events",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Members",
                        column: x => x.BuyerId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketTypeId = table.Column<int>(type: "int", nullable: false, comment: "票券所屬票種，關聯TicketTypes"),
                    CartId = table.Column<int>(type: "int", nullable: false, comment: "票券所屬購物車，關聯Carts"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "選取的票券數量")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartTickets_Carts",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartTickets_TicketTypes",
                        column: x => x.CartId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "訂單建立時間"),
                    ParticipantName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "參加人姓名"),
                    ParticipantEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false, comment: "參加人Email"),
                    ParticipantPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "參加人電話"),
                    TotalMoney = table.Column<decimal>(type: "money", nullable: false, comment: "總價格"),
                    BuyerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "買家名稱"),
                    EventTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "活動標題"),
                    LearnedFrom = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "活動資訊來源")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders",
                        column: x => x.Id,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "票種名稱"),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false, comment: "單價"),
                    PurchaseQuantity = table.Column<int>(type: "int", nullable: false, comment: "購入數量"),
                    OrderDetailId = table.Column<int>(type: "int", nullable: false, comment: "所屬的明細Id，關聯OrderDetials")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTickets_OrderDetails",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Carts_EventId",
                table: "Carts",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_MemberId",
                table: "Carts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CartTickets_CartId",
                table: "CartTickets",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EventId",
                table: "Orders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_OrderDetailId",
                table: "OrderTickets",
                column: "OrderDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartTickets");

            migrationBuilder.DropTable(
                name: "ECPayLog");

            migrationBuilder.DropTable(
                name: "OrderTickets");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6149), new DateTime(2024, 4, 17, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6139), new DateTime(2024, 4, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6132) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndTime", "StartTime" },
                values: new object[] { new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6155), new DateTime(2024, 5, 18, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6153), new DateTime(2024, 5, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6152) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegisterTime",
                value: new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(5933));

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6199), new DateTime(2024, 4, 14, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6207), new DateTime(2024, 3, 16, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6205) });

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateTime", "EndSellTime", "StartSellTime" },
                values: new object[] { new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6209), new DateTime(2024, 4, 14, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6211), new DateTime(2024, 3, 16, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6210) });
        }
    }
}
