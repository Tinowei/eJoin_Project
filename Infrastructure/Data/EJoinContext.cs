using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class EJoinContext : DbContext
{
    public EJoinContext()
    {
    }

    public EJoinContext(DbContextOptions<EJoinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartTicket> CartTickets { get; set; }

    public virtual DbSet<EcpayLog> EcpayLogs { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventTheme> EventThemes { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<GoogleLoginInfo> GoogleLoginInfos { get; set; }

    public virtual DbSet<GoogleMemberRelation> GoogleMemberRelations { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderTicket> OrderTickets { get; set; }

    public virtual DbSet<ReleaseTicket> ReleaseTickets { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:EJoinDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasIndex(e => e.EventId, "IX_Carts_EventId");

            entity.HasIndex(e => e.MemberId, "IX_Carts_MemberId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.CreateTime)
                .HasComment("購物車建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.EventId).HasComment("購物車對應活動，關聯Events");
            entity.Property(e => e.ExpiredTime)
                .HasComment("購物車到期時間")
                .HasColumnType("datetime");
            entity.Property(e => e.LearnedFrom).HasComment("活動資訊來源");
            entity.Property(e => e.MemberId).HasComment("購物車所屬會員，關聯Members");
            entity.Property(e => e.ParticipantEmail)
                .HasMaxLength(320)
                .HasComment("參加人Email");
            entity.Property(e => e.ParticipantName)
                .HasMaxLength(50)
                .HasComment("參加人姓名");
            entity.Property(e => e.ParticipantPhone)
                .HasMaxLength(20)
                .HasComment("參加人電話");

            entity.HasOne(d => d.Event).WithMany(p => p.Carts)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Events");

            entity.HasOne(d => d.Member).WithMany(p => p.Carts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Members");
        });

        modelBuilder.Entity<CartTicket>(entity =>
        {
            entity.HasIndex(e => e.CartId, "IX_CartTickets_CartId");

            entity.HasIndex(e => e.TicketTypeId, "IX_CartTickets_TicketTypeId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.CartId).HasComment("票券所屬購物車，關聯Carts");
            entity.Property(e => e.Quantity).HasComment("選取的票券數量");
            entity.Property(e => e.TicketTypeId).HasComment("票券所屬票種，關聯TicketTypes");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartTickets)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartTickets_Carts");

            entity.HasOne(d => d.TicketType).WithMany(p => p.CartTickets)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartTickets_TicketTypes1");
        });

        modelBuilder.Entity<EcpayLog>(entity =>
        {
            entity.ToTable("ECPayLog");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.CheckMacValue).HasComment("傳送給綠界，檢查碼");
            entity.Property(e => e.ItemName)
                .HasMaxLength(400)
                .HasComment("傳送給綠界，商品名稱");
            entity.Property(e => e.MerchantTradeDate)
                .HasComment("傳送給綠界，特店交易時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MerchantTradeNo)
                .HasMaxLength(20)
                .HasComment("傳送給綠界，特店交易編號");
            entity.Property(e => e.PaymentDate)
                .HasComment("綠界回傳值，付款時間")
                .HasColumnType("datetime");
            entity.Property(e => e.RelateOrderId).HasComment("對應到的OrderId");
            entity.Property(e => e.RtnCode).HasComment("綠界回傳值，交易狀態");
            entity.Property(e => e.RtnMsg)
                .HasMaxLength(200)
                .HasComment("綠界回傳值，交易訊息");
            entity.Property(e => e.TotalAmount).HasComment("傳送給綠界，交易金額");
            entity.Property(e => e.TradeDate)
                .HasComment("綠界回傳值，訂單成立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.TradeNo)
                .HasMaxLength(20)
                .HasComment("綠界回傳值，綠界交易編號");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasIndex(e => e.MemberId, "IX_Events_MemberId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasComment("活動地點的詳細地址");
            entity.Property(e => e.AddressDetail)
                .HasMaxLength(200)
                .HasComment("活動地點的補充說明，可空值");
            entity.Property(e => e.City)
                .HasMaxLength(20)
                .HasComment("活動地點的城市");
            entity.Property(e => e.CoverUrl).HasComment("活動封面圖(單張)");
            entity.Property(e => e.CreateTime)
                .HasComment("活動建立時間，不可變更")
                .HasColumnType("datetime");
            entity.Property(e => e.EndTime)
                .HasComment("活動結束時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Introduction).HasComment("活動簡介");
            entity.Property(e => e.LastEditTime)
                .HasComment("最後編輯時間，可空值")
                .HasColumnType("datetime");
            entity.Property(e => e.Latitude)
                .HasMaxLength(20)
                .HasComment("地址對應的緯度");
            entity.Property(e => e.Longitude)
                .HasMaxLength(20)
                .HasComment("地址對應的經度");
            entity.Property(e => e.MemberId).HasComment("活動建立者，關聯Members");
            entity.Property(e => e.StartTime)
                .HasComment("活動開始時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasComment("活動狀態：1.草稿，2.上架，3.結束，4.下架");
            entity.Property(e => e.Summary).HasComment("活動摘要");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasComment("活動標題");

            entity.HasOne(d => d.Member).WithMany(p => p.Events)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Events_Members");
        });

        modelBuilder.Entity<EventTheme>(entity =>
        {
            entity.HasIndex(e => e.EventId, "IX_EventThemes_EventId");

            entity.HasIndex(e => e.ThemeId, "IX_EventThemes_ThemeId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.EventId).HasComment("活動Id，關聯Events");
            entity.Property(e => e.ThemeId).HasComment("主題Id，關聯Themes");

            entity.HasOne(d => d.Event).WithMany(p => p.EventThemes)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventThemes_Events");

            entity.HasOne(d => d.Theme).WithMany(p => p.EventThemes)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventThemes_Themes");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasIndex(e => e.BeingFollowedId, "IX_Follows_BeingFollowedId");

            entity.HasIndex(e => e.FollowerId, "IX_Follows_FollowerId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.BeingFollowedId).HasComment("被跟隨者，關聯Members");
            entity.Property(e => e.FollowerId).HasComment("跟隨者，關聯Members");

            entity.HasOne(d => d.BeingFollowed).WithMany(p => p.FollowBeingFolloweds)
                .HasForeignKey(d => d.BeingFollowedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Follows_Members1");

            entity.HasOne(d => d.Follower).WithMany(p => p.FollowFollowers)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Follows_Members");
        });

        modelBuilder.Entity<GoogleLoginInfo>(entity =>
        {
            entity.ToTable("GoogleLoginInfo");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.CreateTime)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Gamil)
                .HasMaxLength(320)
                .HasComment("Google回傳Gmail");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("Google回傳Name");
            entity.Property(e => e.NameIdentifier).HasComment("Google回傳NameIdentifier欄位");
        });

        modelBuilder.Entity<GoogleMemberRelation>(entity =>
        {
            entity.ToTable("GoogleMemberRelation");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.GoogleLoginInfoId).HasComment("Google紀錄表Id，關聯GoogleLoginInfo");
            entity.Property(e => e.MemberId).HasComment("會員Id，關連Members");

            entity.HasOne(d => d.GoogleLoginInfo).WithMany(p => p.GoogleMemberRelations)
                .HasForeignKey(d => d.GoogleLoginInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoogleMemberRelation_GoogleLoginInfo");

            entity.HasOne(d => d.Member).WithMany(p => p.GoogleMemberRelations)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoogleMemberRelation_Members");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasIndex(e => e.EventId, "IX_Likes_EventId");

            entity.HasIndex(e => e.MemberId, "IX_Likes_MemberId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.EventId).HasComment("被喜歡的活動，關聯Events");
            entity.Property(e => e.MemberId).HasComment("會員，關聯Members");

            entity.HasOne(d => d.Event).WithMany(p => p.Likes)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Events");

            entity.HasOne(d => d.Member).WithMany(p => p.Likes)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Members");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasComment("詳細地址，可空值");
            entity.Property(e => e.AvatarUrl).HasComment("大頭照網址，可空值");
            entity.Property(e => e.Birthday)
                .HasComment("生日，可空值")
                .HasColumnType("date");
            entity.Property(e => e.City)
                .HasMaxLength(20)
                .HasComment("城市，可空值");
            entity.Property(e => e.CoverUrl).HasComment("封面圖片網址，可空值");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasComment("個人簡介，可空值");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .HasComment("顯示名稱，可空值");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasComment("驗證與登入用信箱，不可變更");
            entity.Property(e => e.Gender).HasComment("性別，可空值");
            entity.Property(e => e.IsDelete).HasComment("是否刪除");
            entity.Property(e => e.LastEditTime)
                .HasComment("最後編輯時間，可空值")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("帳號用姓名");
            entity.Property(e => e.Password).HasComment("雜湊後的密碼");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasComment("電話");
            entity.Property(e => e.RegisterTime)
                .HasComment("註冊時間，不可變更")
                .HasColumnType("datetime");
            entity.Property(e => e.Relationship).HasComment("感情狀態，可空值");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.BuyerId, "IX_Orders_BuyerId");

            entity.HasIndex(e => e.EventId, "IX_Orders_EventId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.BuyerId).HasComment("買家Id，關聯Members");
            entity.Property(e => e.EventId).HasComment("活動Id，關聯Events");
            entity.Property(e => e.ExpiredTime)
                .HasComment("待付款的到期時間")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(50)
                .HasComment("訂單編號");
            entity.Property(e => e.Status).HasComment("訂單狀態，1為待付款，2為已付款");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Members");

            entity.HasOne(d => d.Event).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Events");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("自動生成Id");
            entity.Property(e => e.BuyerName)
                .HasMaxLength(50)
                .HasComment("買家名稱");
            entity.Property(e => e.CreateTime)
                .HasComment("訂單建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.EventTitle)
                .HasMaxLength(50)
                .HasComment("活動標題");
            entity.Property(e => e.LearnedFrom).HasComment("活動資訊來源");
            entity.Property(e => e.ParticipantEmail)
                .HasMaxLength(320)
                .HasComment("參加人Email");
            entity.Property(e => e.ParticipantName)
                .HasMaxLength(50)
                .HasComment("參加人姓名");
            entity.Property(e => e.ParticipantPhone)
                .HasMaxLength(20)
                .HasComment("參加人電話");
            entity.Property(e => e.TotalMoney)
                .HasComment("總價格")
                .HasColumnType("money");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.OrderDetail)
                .HasForeignKey<OrderDetail>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder.Entity<OrderTicket>(entity =>
        {
            entity.HasIndex(e => e.OrderDetailId, "IX_OrderTickets_OrderDetailId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.OrderDetailId).HasComment("所屬的明細Id，關聯OrderDetials");
            entity.Property(e => e.PurchaseQuantity).HasComment("購入數量");
            entity.Property(e => e.TicketTypeId).HasComment("原票種的Id");
            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50)
                .HasComment("票種名稱");
            entity.Property(e => e.UnitPrice)
                .HasComment("單價")
                .HasColumnType("money");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.OrderTickets)
                .HasForeignKey(d => d.OrderDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderTickets_OrderDetails");
        });

        modelBuilder.Entity<ReleaseTicket>(entity =>
        {
            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.ChangedTime)
                .HasComment("異動時間，可空值")
                .HasColumnType("datetime");
            entity.Property(e => e.EventId).HasComment("票券所屬活動，關聯Events");
            entity.Property(e => e.ExpireTime)
                .HasComment("到期時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasComment("票券持有者，關聯Members");
            entity.Property(e => e.OrderId).HasComment("票券所屬訂單，關聯Orders");
            entity.Property(e => e.ParticipanPhone)
                .HasMaxLength(20)
                .HasComment("參加人電話");
            entity.Property(e => e.ParticipantEmail)
                .HasMaxLength(320)
                .HasComment("參加人Email");
            entity.Property(e => e.ParticipantName)
                .HasMaxLength(50)
                .HasComment("參加人姓名");
            entity.Property(e => e.ReleaseTicketNumber).HasComment("票券編號");
            entity.Property(e => e.Staff)
                .HasMaxLength(50)
                .HasComment("核銷人員");
            entity.Property(e => e.Status).HasComment("票券異動狀態，");
            entity.Property(e => e.TicketTypeId).HasComment("票券所屬種類，關聯TicketTypes");

            entity.HasOne(d => d.Event).WithMany(p => p.ReleaseTickets)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReleaseTickets_Events");

            entity.HasOne(d => d.Member).WithMany(p => p.ReleaseTickets)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReleaseTickets_Members");

            entity.HasOne(d => d.Order).WithMany(p => p.ReleaseTickets)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReleaseTickets_Orders");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.IconUrl).HasComment("主題圖片位址，應在本地端");
            entity.Property(e => e.ThemeName)
                .HasMaxLength(10)
                .HasComment("主題名稱");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TicketType");

            entity.HasIndex(e => e.EventId, "IX_TicketTypes_EventId");

            entity.Property(e => e.Id).HasComment("自動生成Id");
            entity.Property(e => e.CreateTime)
                .HasComment("建立時間，不可變更")
                .HasColumnType("datetime");
            entity.Property(e => e.EndSellTime)
                .HasComment("結束販售時間")
                .HasColumnType("datetime");
            entity.Property(e => e.EventId).HasComment("所屬活動Id，關聯Events");
            entity.Property(e => e.MaxPurchase).HasComment("單次最大購買數量，可空值，代表無限制");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("票種名稱");
            entity.Property(e => e.ReleaseAmount).HasComment("釋出總數");
            entity.Property(e => e.StartSellTime)
                .HasComment("開始販售時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Stock).HasComment("庫存");
            entity.Property(e => e.UnitPrice)
                .HasComment("單價")
                .HasColumnType("money");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketTypes)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketTypes_Events");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
