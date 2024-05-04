using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public partial class EJoinContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // Seed data for Members
            modelBuilder.Entity<Member>().HasData(
               new Member
               {
                   Id = 1,
                   Name = "John Doe",
                   Email = "john.doe@example.com",
                   DisplayName = "John",
                   CoverUrl = "https://example.com/cover1.jpg",
                   AvatarUrl = "https://example.com/avatar1.jpg",
                   Phone = "1234567890",
                   Description = "Software Developer",
                   Birthday = new DateTime(1990, 1, 1),
                   Gender = 1, // Assuming 1 for Male
                   Relationship = 2, // Assuming 2 for Single
                   City = "Taipei",
                   Address = "123 Main St",
                   Password = "hashedPassword",
                   RegisterTime = DateTime.Now,
                   LastEditTime = null,
                   IsDelete = false
               },
               new Member
               {
                   Id = 2,
                   Name = "Jane Doe",
                   Email = "jane.doe@example.com",
                   DisplayName = "Jane",
                   CoverUrl = "https://example.com/cover2.jpg",
                   AvatarUrl = "https://example.com/avatar2.jpg",
                   Phone = "0987654321",
                   Description = "Graphic Designer",
                   Birthday = new DateTime(1992, 2, 2),
                   Gender = 2, // Assuming 2 for Female
                   Relationship = 1, // Assuming 1 for In a Relationship
                   City = "Taichung",
                   Address = "456 Main St",
                   Password = "hashedPassword2",
                   RegisterTime = DateTime.Now,
                   LastEditTime = null,
                   IsDelete = false
               }
           );

            // Seed data for Themes
            modelBuilder.Entity<Theme>().HasData(
                new Theme { Id = 1, ThemeName = "課程", IconUrl = "music_icon.png" },
                new Theme { Id = 2, ThemeName = "公益", IconUrl = "sports_icon.png" }
            );

            // Seed data for Events
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Tech Conference",
                    CoverUrl = "https://example.com/cover1.jpg",
                    MemberId = 1,
                    StartTime = DateTime.Now.AddMonths(1),
                    EndTime = DateTime.Now.AddMonths(1).AddDays(2),
                    City = "Taipei",
                    Address = "123 Main St",
                    AddressDetail = "Floor 5",
                    Summary = "A tech conference for developers.",
                    Introduction = "Join us for a day of learning and networking.",
                    Latitude = "25.0329",
                    Longitude = "121.5654",
                    Status = 2, // Assuming 2 for Published
                    CreateTime = DateTime.Now,
                    LastEditTime = null
                },
                new Event
                {
                    Id = 2,
                    Title = "Music Festival",
                    CoverUrl = "https://example.com/cover2.jpg",
                    MemberId = 2,
                    StartTime = DateTime.Now.AddMonths(2),
                    EndTime = DateTime.Now.AddMonths(2).AddDays(3),
                    City = "Taichung",
                    Address = "456 Main St",
                    AddressDetail = "Ground Floor",
                    Summary = "A music festival featuring local bands.",
                    Introduction = "Experience the best of local music.",
                    Latitude = "24.1754",
                    Longitude = "120.6899",
                    Status = 2, // Assuming 2 for Published
                    CreateTime = DateTime.Now,
                    LastEditTime = null
                }
            );

            // Seed data for EventThemes
            modelBuilder.Entity<EventTheme>().HasData(
                new EventTheme { Id = 1, EventId = 1, ThemeId = 1 },
                new EventTheme { Id = 2, EventId = 2, ThemeId = 2 }
            );

            // Seed data for Likes
            modelBuilder.Entity<Like>().HasData(
                new Like { Id = 1, MemberId = 1, EventId = 1 },
                new Like { Id = 2, MemberId = 2, EventId = 1 }
            );

            // Seed data for TicketTypes
            modelBuilder.Entity<TicketType>().HasData(
                new TicketType
                {
                    Id = 1,
                    EventId = 1,
                    CreateTime = DateTime.Now,
                    ReleaseAmount = 100,
                    Name = "General Admission",
                    UnitPrice = 50.00m,
                    StartSellTime = DateTime.Now.AddDays(1),
                    EndSellTime = DateTime.Now.AddDays(30),
                    MaxPurchase = 5,
                    Stock = 100
                },
                new TicketType
                {
                    Id = 2,
                    EventId = 1,
                    CreateTime = DateTime.Now,
                    ReleaseAmount = 50,
                    Name = "VIP",
                    UnitPrice = 100.00m,
                    StartSellTime = DateTime.Now.AddDays(1),
                    EndSellTime = DateTime.Now.AddDays(30),
                    MaxPurchase = 2,
                    Stock = 50
                }
            );

            // Seed data for Follows
            modelBuilder.Entity<Follow>().HasData(
               new Follow { Id = 1, FollowerId = 1, BeingFollowedId = 2 },
               new Follow { Id = 2, FollowerId = 2, BeingFollowedId = 1 }
           );
        }

    }

}
