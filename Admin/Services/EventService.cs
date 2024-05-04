using Admin.Helpers;
using Admin.Models;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.EventService;
using ApplicationCore.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Admin.Services;

public class EventService : IEventService
{
    
    private readonly ConnectDBHelper _dbHelper;

    public EventService(IConfiguration configuration)
    {
        _dbHelper = ConnectDBHelper.GetInstance(configuration);
    }

    public  List<Event> GetEvents()
    {
        List<Event> events = null;
        
        //use dapper
        using (var connection = _dbHelper.OpenConnection())
        {
            var sql = @"select * from Events";
            events = connection.Query<Event>(sql).ToList();
        }

        return events;
    }

    public async Task<int> UpdateEventAsync(int id, string status, string title)
    {
        int affectedRow = 0;
        using (var connection =  _dbHelper.OpenConnection())
        {
            var sql = @"UPDATE Events SET Title = @Title, Status = @Status , LastEditTime = @LastEditTime WHERE Id = @Id";
            affectedRow = connection.Execute(sql, new
            {
                Id = id,
                Title = title,
                Status = GetStatusDescription.ChangeDesToNumber(status),
                LastEditTime= CustomUtcNow.CurrentTime
            });
        }

        return affectedRow;
    }
    
    //todo: 新增一個方法，用dapper取回我要的活動資訊，轉成Json的資料格式，傳回前端並下載下來到本地

    public async Task<List<EventDTO>> CreateJsonEvents()
    {
        List<EventDTO> events = null;
        
        using (var connection = _dbHelper.OpenConnection())
        {
            var sql = @"SELECT
                        e.Id,
                        e.Title,
                        e.CoverUrl,
                        e.StartTime,
                        e.EndTime,
                        e.City,
                        e.Address,
                        e.AddressDetail,
                        e.Summary,
                        e.Introduction,
                        STRING_AGG(t.ThemeName, ', ') AS Themes
                    FROM Events e
                    INNER JOIN EventThemes et ON e.Id = et.EventId
                    INNER JOIN Themes t ON et.ThemeId = t.Id
                    Where e.Status = 2
                    GROUP BY e.Id, e.Title, e.CoverUrl, e.StartTime, e.EndTime, e.City, e.Address, e.AddressDetail, e.Summary, e.Introduction;";
            events = connection.Query<EventDTO>(sql).ToList();
        }

        return events;
    }
}

public class EventDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string AddressDetail { get; set; }
    public string Summary { get; set; }
    public string Introduction { get; set; }
    public string Themes { get; set; }
}