
using ApplicationCore.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Tracing;

namespace Admin.Services
{
    public class DashboardService
    {
        //連線字串
        private readonly string connectionString = string.Empty;

        private List<string> MonthList = new List<string>
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            };

        public DashboardService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("EJoinDB") ?? string.Empty;
        }

        public DashBoardData GetData()
        {
            var data = new DashBoardData();
            data.EventCount = GetEventCount();
            data.MemberCount = GetMemberCount();
            data.OrderCount = GetOrderCount();
            data.TotalPrice = GetTotalPrice();
            data.AmountGrowths = GetAmountGrowths();
            data.MemberGrowths = GetMemberGrowths();
            data.ThemeCategories = GetThemeCategories();
            data.Top5Events = GetTop5Events();

            return data;
        }

        //活動總數
        private int GetEventCount()
        {
            var eventCount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT COUNT(1) AS Count FROM [Events]
                    WHERE STATUS IN(2,3,4)
                ";

                eventCount = connection.ExecuteScalar<int>(sql);
            }

            return eventCount;
        }

        //會員總數
        private int GetMemberCount()
        {
            var memberCount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT COUNT(1) AS Count FROM [Members]
                ";

                memberCount = connection.ExecuteScalar<int>(sql);
            }
            return memberCount;
        }

        //訂單總數
        private int GetOrderCount()
        {
            var orderCount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT COUNT(1) AS Count FROM [Orders]
                    WHERE Status = 2
                ";

                orderCount = connection.ExecuteScalar<int>(sql);
            }

            return orderCount;
        }

        //今年營業總金額
        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT SUM([OrderDetails].TotalMoney) AS TotalMoney FROM [OrderDetails]
                    INNER JOIN [Orders]
                    ON [OrderDetails].Id = [Orders].Id AND [Orders].Status = 2
                ";

                totalPrice = connection.ExecuteScalar<decimal>(sql);
            }

            return totalPrice;
        }

        //收益成長(每月營業總金額)
        private List<AmountGrowth> GetAmountGrowths()
        {
            var list = new List<AmountGrowth>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT 
                        DATEPART(MONTH, [OrderDetails].CreateTime) AS Month,
                        SUM([OrderDetails].TotalMoney) AS TotalMoney 
                    FROM [OrderDetails]
                    INNER JOIN [Orders]
                    ON [OrderDetails].Id = [Orders].Id 
                    AND [Orders].Status = 2
                    GROUP BY
                      DATEPART(MONTH, [OrderDetails].CreateTime)
                ";

                var result = connection.Query<OrderTotalMoneyByMonth>(sql).ToList();

                for(var i = 0; i< MonthList.Count; i++)
                {
                    var month = MonthList[i];
                    var monthTotalMoney = result.FirstOrDefault(x => x.Month == i + 1);
                    decimal totalMoney = 0;
                    if (monthTotalMoney != null)
                    {
                        totalMoney = monthTotalMoney.TotalMoney;
                    }

                    list.Add(new AmountGrowth(month, totalMoney));
                } 

            }

            return list;
        }

        //會員成長(每月會員總數)
        private List<MemberGrowth> GetMemberGrowths()
        {
            var list = new List<MemberGrowth>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT 
                        DATEPART(MONTH, [Members].RegisterTime) AS Month,
                        COUNT(1) AS Count
                    FROM [Members]
                    GROUP BY
                      DATEPART(MONTH, [Members].RegisterTime)
                ";

                var result = connection.Query<MemberCountByMonth>(sql).ToList();

                for (var i = 0; i < MonthList.Count; i++)
                {
                    var month = MonthList[i];
                    var monthMemberCount = result.FirstOrDefault(x => x.Month == i + 1);
                    int count = 0;
                    if (monthMemberCount != null)
                    {
                        count = monthMemberCount.Count;
                    }

                    list.Add(new MemberGrowth(month, count));
                }

            }

            return list;
        }

        //所有活動的主題分布
        private List<ThemeCategory> GetThemeCategories()
        {
            var list = new List<ThemeCategory>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    /*找有建立活動主題的數量*/
                    SELECT T.ThemeName AS Name,COUNT(1) AS Count
                    FROM [Events] AS EV
                    INNER JOIN EventThemes AS ET ON EV.Id = ET.EventId
                    INNER JOIN Themes AS T ON ET.ThemeId = T.Id
                    GROUP BY T.ThemeName

                    UNION ALL /* 資料合併 */

                    /*找沒有被建立活動的主題*/
                    SELECT ThemeName AS Name, 0 Count FROM Themes AS T
                    WHERE T.Id NOT IN(
	                    SELECT ET.ThemeId FROM EventThemes AS ET
	                    GROUP BY ET.ThemeId
                    )
                ";

                list = connection.Query<ThemeCategory>(sql).ToList();

            }

            return list;
        }

        //依銷售金額做TOP5排名
        private List<Top5Event> GetTop5Events()
        {
            var list = new List<Top5Event>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"
                    SELECT TOP 5 WITH TIES
	                    E.Title AS EventName, 
	                    M.Name AS MemberName, 
	                    COUNT(1) AS SalesVolume, 
	                    SUM(OD.TotalMoney) AS TotalPrice
                    FROM [Events] AS E
                    INNER JOIN Orders AS O ON E.Id = O.EventId AND O.Status = 2
                    INNER JOIN Members AS M ON O.BuyerId = M.Id
                    INNER JOIN OrderDetails AS OD ON O.Id = OD.Id
                    GROUP BY E.Title, M.Name  
                    Order By SUM(OD.TotalMoney) DESC
                ";

                list = connection.Query<Top5Event>(sql).ToList();

            }

            return list;
        }
    }

    public class DashBoardData
    {
        public DashBoardData()
        {
            var monthList = new List<string>
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            };


            //預設值
            this.EventCount = 2;
            this.OrderCount = 10;
            this.MemberCount = 2;
            this.TotalPrice = 100;

            this.AmountGrowths = new List<AmountGrowth>();
            var groupNumber = 1;
            foreach (var month in monthList)
            {
                this.AmountGrowths.Add(new AmountGrowth(month, 10000 * groupNumber));
                groupNumber++;
            }

            this.MemberGrowths = new List<MemberGrowth>();
            foreach (var month in monthList)
            {
                this.MemberGrowths.Add(new MemberGrowth(month, 2));
            }

            this.ThemeCategories = new List<ThemeCategory>()
            {
                new ThemeCategory("美食", 10),
                new ThemeCategory("旅遊", 5),
                new ThemeCategory("攝影", 3)
            };

            this.Top5Events = new List<Top5Event>()
            {
                new Top5Event
                {
                    EventName = "Test",
                    MemberName = "Test",
                    SalesVolume = 100,
                    TotalPrice = 100000,
                }
            };
        }

        public int EventCount { get; set; }
        public int OrderCount { get; set; }
        public int MemberCount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<AmountGrowth> AmountGrowths { get; set; }
        public List<MemberGrowth> MemberGrowths { get; set; }
        public List<ThemeCategory> ThemeCategories { get; set; }
        public List<Top5Event> Top5Events { get; set; }
    }

    public class OrderTotalMoneyByMonth
    {
        public int Month { get; set; }
        public decimal TotalMoney { get; set; }
    }

    public class AmountGrowth
    {
        public AmountGrowth(string month, decimal amount)
        {
            this.Month = month;
            this.Amount = amount;
        }
        public string Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class MemberCountByMonth
    {
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class MemberGrowth
    {
        public MemberGrowth(string month, int memberCount)
        {
            this.Month = month;
            this.MemberCount = memberCount;
        }
        public string Month { get; set; }
        public int MemberCount { get; set; }
    }

    public class ThemeCategory
    {
        public ThemeCategory(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class Top5Event
    {
        public string EventName { get; set; }
        public string MemberName { get; set; }
        public int SalesVolume { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

