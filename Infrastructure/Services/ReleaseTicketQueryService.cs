using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;
using Infrastructure.Models;

namespace Infrastructure.Services;

public class ReleaseTicketQueryService : EfRepository<ReleaseTicket>,IReleaseTicketQueryService
{
    private readonly IRepository<TicketType> _ticketTypeRepo;
    public ReleaseTicketQueryService(EJoinContext dbContext,IRepository<TicketType> ticketType) : base(dbContext)
    {
        _ticketTypeRepo = ticketType;
    }
    
    
    /// <summary>
    /// 找到所有符合的數量，狀態紀錄要是0->代表沒有使用過
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="ticketTypeId"></param>
    /// <returns></returns>
    public int GetTicketCount(int memberId, int eventId, int ticketTypeId)
    {
        return DbContext.ReleaseTickets.Count(rt =>
            rt.MemberId==memberId && rt.TicketTypeId == ticketTypeId && rt.EventId == eventId && rt.Status == 0);
    }
    
    public List<ReleaseTicket> GetTicketsByNumber(int quantity,int eventId,int ticketTypeId,int memberId)
    {
        //找出符合的已發行票券
        //條件1:找到活動票券種類相同且活動id一樣的
        //條件2:Status狀態欄位=0(未使用的)
        var selectedTickets = DbContext.ReleaseTickets
            .Where(rt => rt.MemberId == memberId && rt.TicketTypeId == ticketTypeId && rt.EventId == eventId && rt.Status == 0)
            .Take(quantity)
            .ToList();
        return selectedTickets;
    }

    public List<ReleaseTicket> GetAll(int userId)
    {
        int testuserId = 2;
        var result = DbContext.ReleaseTickets.Where(rt => rt.MemberId == testuserId).Select(rt => new ReleaseTicket()
        {
            MemberId = rt.MemberId,
            TicketTypeId = rt.TicketTypeId,
            EventId = rt.EventId,
            ReleaseTicketNumber = rt.ReleaseTicketNumber,
            OrderId = rt.OrderId,
            ParticipantEmail = rt.ParticipantEmail,
            ParticipanPhone = rt.ParticipanPhone,
            ParticipantName = rt.ParticipantName,
            ExpireTime = rt.ExpireTime
        }).ToList();
        return result;
    }

    /// <summary>
    /// 查詢出該票券的活動主辦方id
    /// </summary>
    /// <param name="ticketNumber"></param>
    /// <returns></returns>
    public int GetTicketHostId(string ticketNumber)
    {
        //將票券編號做查詢，找到eventId，再透過eventId查找活動的主辦方Id
        var eventId =
            DbContext.ReleaseTickets.Where(rt => rt.ReleaseTicketNumber == ticketNumber)
                                    .Select(rt => rt.EventId)
                                    .FirstOrDefault();
        
        var eventHostId = DbContext.Events.Where(e => e.Id == eventId)
                                                       .Select(e=>e.MemberId).FirstOrDefault();
        
        return eventHostId;
    }

    public List<ReleaseTicketSummaryDTO> GetReleaseTicketSummaries(int userId,int page,int pageSize)
    {
        //找到該持有者的所有未使用的已發行票券
        // var releaseTicketTargets = List(rt => rt.MemberId == userId && rt.Status == 0);
        var releaseTicketTargets=DbContext.ReleaseTickets.Include(rt=>rt.Event).Where(rt => rt.MemberId == userId && rt.Status == 0).ToList();
        //找到該持有者所有已發行票券中所有的ticketTypeId
        var ticketTypeIds = releaseTicketTargets.Select(rt => rt.TicketTypeId).Distinct().ToList();
        var ticketTypeTargets = _ticketTypeRepo.List(tt => ticketTypeIds.Contains(tt.Id));

        
        //依據EventId,TicketTypeId,ExpireTime GroupBy ->實現前端畫面一張票券卡片含有複數張票券
        //ticketTypeTargets.TicketTypeId == 分組後的ticketTypeId ->如果找到，抓取他的票券名稱
        var result = releaseTicketTargets
            .GroupBy(rt => new { rt.EventId, rt.TicketTypeId, rt.ExpireTime })
            .Select(g => new ReleaseTicketSummaryDTO
            {
                EventId = g.Key.EventId,
                EventName = g.FirstOrDefault().Event.Title,
                TicketTypeId = g.Key.TicketTypeId,
                TicketTypeName = ticketTypeTargets.FirstOrDefault(tt => tt.Id == g.Key.TicketTypeId)?.Name,
                Quantity = g.Count(),
                ExpireTime = g.Key.ExpireTime,
                ParticipantName = g.FirstOrDefault().ParticipantName,
                ParticipantEmail = g.FirstOrDefault().ParticipantEmail,
                ParticipantPhone = g.FirstOrDefault().ParticipanPhone,
            }).OrderByDescending(rt=>rt.EventId).ToList();
        
        // 分頁分批抓取
        var pagedResult = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return pagedResult;

        return result;
    }

    public List<UsedTicketsSummaryDTO> GetUsedTickets(int userId,int page,int pageSize)
    {
        //找到所有該會員已經使用過的票券，Status=1的表示已使用過
        // var usedTickets = List(rt => rt.MemberId == userId && rt.Status == 1);
        var usedTickets = DbContext.ReleaseTickets.Include(rt=>rt.Event).Where(rt => rt.MemberId == userId && rt.Status == 1).ToList();
        
        //找到該持有者所有已使用過的已發行票券中所有的ticketTypeId
        var ticketTypeIds = usedTickets.Select(rt => rt.TicketTypeId).Distinct().ToList();
        var ticketTypeTargets = _ticketTypeRepo.List(tt => ticketTypeIds.Contains(tt.Id));
        
        
        
        //依據EventId,TicketTypeId,ChangedTime GroupBy -> 把同一時間使用的票券集合起來
        var result = usedTickets.GroupBy(rt => new { rt.EventId, rt.TicketTypeId, rt.ChangedTime })
            .Select(g => new UsedTicketsSummaryDTO()
            {
                EventId = g.Key.EventId,
                EventName = g.FirstOrDefault().Event.Title,
                TicketTypeId = g.Key.TicketTypeId,
                TicketTypeName = ticketTypeTargets.FirstOrDefault(tt => tt.Id == g.Key.TicketTypeId)?.Name,
                ChangedTime = g.Key.ChangedTime,
                Quantity = g.Count(),
                ParticipantName = g.FirstOrDefault().ParticipantName,
                ParticipantEmail = g.FirstOrDefault().ParticipantEmail,
                ParticipantPhone = g.FirstOrDefault().ParticipanPhone,
            }).OrderByDescending(dto=>dto.ChangedTime).ToList();
        // 分頁分批抓取
        var pagedResult = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return pagedResult;
        // return result;
    }

    public int GetTicketsCount(int userId)
    {
        var result = GetReleaseTicketSummaries(userId, 1, int.MaxValue).Count;
        return result;
    }

    public int GetUsedTicketsCount(int userId)
    {
        var result = GetUsedTickets(userId,1, int.MaxValue).Count;
        return result;
    }
    
    
    
    
    
    
    //test pageList<T>
    public async Task<IPagedList<ReleaseTicketSummaryDTO>> GetReleaseTicketListByPageAsync(
        ReleaseTicketParameters releaseTicketParameters)
    {
        var releaseTicketTargets=DbContext.ReleaseTickets.Include(rt=>rt.Event).Where(rt => rt.MemberId == releaseTicketParameters.MemberId && rt.Status == 0);
        
        //找到該持有者所有已發行票券中所有的ticketTypeId
        var ticketTypeIds = releaseTicketTargets.Select(rt => rt.TicketTypeId).Distinct().ToList();
        var ticketTypeTargets = _ticketTypeRepo.List(tt => ticketTypeIds.Contains(tt.Id));

        
        //依據EventId,TicketTypeId,ExpireTime GroupBy ->實現前端畫面一張票券卡片含有複數張票券
        //ticketTypeTargets.TicketTypeId == 分組後的ticketTypeId ->如果找到，抓取他的票券名稱
        var result = releaseTicketTargets
            .GroupBy(rt => new { rt.EventId, rt.TicketTypeId, rt.ExpireTime })
            .Select(g => new ReleaseTicketSummaryDTO
            {
                EventId = g.Key.EventId,
                EventName = g.FirstOrDefault().Event.Title,
                TicketTypeId = g.Key.TicketTypeId,
                TicketTypeName = ticketTypeTargets.FirstOrDefault(tt => tt.Id == g.Key.TicketTypeId)!=null ?ticketTypeTargets.FirstOrDefault(tt => tt.Id == g.Key.TicketTypeId).Name : null,
                Quantity = g.Count(),
                ExpireTime = g.Key.ExpireTime,
                ParticipantName = g.FirstOrDefault().ParticipantName,
                ParticipantEmail = g.FirstOrDefault().ParticipantEmail,
                ParticipantPhone = g.FirstOrDefault().ParticipanPhone,
            });
        
        var pagedList = new PagedList<ReleaseTicketSummaryDTO>();
        await pagedList.ReadAsync(result,
            releaseTicketParameters.PageNumber,
            releaseTicketParameters.PageSize);
        
        return pagedList;
    }
}

