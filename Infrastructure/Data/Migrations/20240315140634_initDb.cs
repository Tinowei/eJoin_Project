using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "帳號用姓名"),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false, comment: "驗證與登入用信箱，不可變更"),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "顯示名稱，可空值"),
                    CoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "封面圖片網址，可空值"),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "大頭照網址，可空值"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "電話"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "個人簡介，可空值"),
                    Birthday = table.Column<DateTime>(type: "date", nullable: true, comment: "生日，可空值"),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true, comment: "性別，可空值"),
                    Relationship = table.Column<byte>(type: "tinyint", nullable: true, comment: "感情狀態，可空值"),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "城市，可空值"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "詳細地址，可空值"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "雜湊後的密碼"),
                    RegisterTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "註冊時間，不可變更"),
                    LastEditTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最後編輯時間，可空值"),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, comment: "是否刪除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "主題名稱"),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "主題圖片位址，應在本地端")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "活動標題"),
                    CoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "活動封面圖(單張)"),
                    MemberId = table.Column<int>(type: "int", nullable: false, comment: "活動建立者，關聯Members"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "活動開始時間"),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "活動結束時間"),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "活動地點的城市"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "活動地點的詳細地址"),
                    AddressDetail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "活動地點的補充說明，可空值"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "活動摘要"),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "活動簡介"),
                    Latitude = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "地址對應的緯度"),
                    Longitude = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "地址對應的經度"),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, comment: "活動狀態：1.草稿，2.上架，3.結束，4.下架"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "活動建立時間，不可變更"),
                    LastEditTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最後編輯時間，可空值")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Members",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FollowerId = table.Column<int>(type: "int", nullable: false, comment: "跟隨者，關聯Members"),
                    BeingFollowedId = table.Column<int>(type: "int", nullable: false, comment: "被跟隨者，關聯Members")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_Members",
                        column: x => x.FollowerId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Follows_Members1",
                        column: x => x.BeingFollowedId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "活動Id，關聯Events"),
                    ThemeId = table.Column<int>(type: "int", nullable: false, comment: "主題Id，關聯Themes")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventThemes_Events",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventThemes_Themes",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false, comment: "會員，關聯Members"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "被喜歡的活動，關聯Events")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Events",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Members",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "自動生成Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "所屬活動Id，關聯Events"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "建立時間，不可變更"),
                    ReleaseAmount = table.Column<int>(type: "int", nullable: false, comment: "釋出總數"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "票種名稱"),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false, comment: "單價"),
                    StartSellTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "開始販售時間"),
                    EndSellTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "結束販售時間"),
                    MaxPurchase = table.Column<int>(type: "int", nullable: true, comment: "單次最大購買數量，可空值，代表無限制"),
                    Stock = table.Column<int>(type: "int", nullable: false, comment: "庫存")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketTypes_Events",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Address", "AvatarUrl", "Birthday", "City", "CoverUrl", "Description", "DisplayName", "Email", "Gender", "IsDelete", "LastEditTime", "Name", "Password", "Phone", "RegisterTime", "Relationship" },
                values: new object[,]
                {
                    { 1, "123 Main St", "https://example.com/avatar1.jpg", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taipei", "https://example.com/cover1.jpg", "Software Developer", "John", "john.doe@example.com", (byte)1, false, null, "John Doe", "hashedPassword", "1234567890", new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(5918), (byte)2 },
                    { 2, "456 Main St", "https://example.com/avatar2.jpg", new DateTime(1992, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taichung", "https://example.com/cover2.jpg", "Graphic Designer", "Jane", "jane.doe@example.com", (byte)2, false, null, "Jane Doe", "hashedPassword2", "0987654321", new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(5933), (byte)1 }
                });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "IconUrl", "ThemeName" },
                values: new object[,]
                {
                    { 1, "music_icon.png", "課程" },
                    { 2, "sports_icon.png", "公益" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Address", "AddressDetail", "City", "CoverUrl", "CreateTime", "EndTime", "Introduction", "LastEditTime", "Latitude", "Longitude", "MemberId", "StartTime", "Status", "Summary", "Title" },
                values: new object[,]
                {
                    { 1, "123 Main St", "Floor 5", "Taipei", "https://example.com/cover1.jpg", new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6149), new DateTime(2024, 4, 17, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6139), "Join us for a day of learning and networking.", null, "25.0329", "121.5654", 1, new DateTime(2024, 4, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6132), (byte)2, "A tech conference for developers.", "Tech Conference" },
                    { 2, "456 Main St", "Ground Floor", "Taichung", "https://example.com/cover2.jpg", new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6155), new DateTime(2024, 5, 18, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6153), "Experience the best of local music.", null, "24.1754", "120.6899", 2, new DateTime(2024, 5, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6152), (byte)2, "A music festival featuring local bands.", "Music Festival" }
                });

            migrationBuilder.InsertData(
                table: "Follows",
                columns: new[] { "Id", "BeingFollowedId", "FollowerId" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "EventThemes",
                columns: new[] { "Id", "EventId", "ThemeId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "Id", "EventId", "MemberId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "Id", "CreateTime", "EndSellTime", "EventId", "MaxPurchase", "Name", "ReleaseAmount", "StartSellTime", "Stock", "UnitPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6199), new DateTime(2024, 4, 14, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6207), 1, 5, "General Admission", 100, new DateTime(2024, 3, 16, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6205), 100, 50.00m },
                    { 2, new DateTime(2024, 3, 15, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6209), new DateTime(2024, 4, 14, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6211), 1, 2, "VIP", 50, new DateTime(2024, 3, 16, 22, 6, 33, 951, DateTimeKind.Local).AddTicks(6210), 50, 100.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_MemberId",
                table: "Events",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EventThemes_EventId",
                table: "EventThemes",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventThemes_ThemeId",
                table: "EventThemes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_BeingFollowedId",
                table: "Follows",
                column: "BeingFollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_EventId",
                table: "Likes",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_MemberId",
                table: "Likes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_EventId",
                table: "TicketTypes",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventThemes");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
