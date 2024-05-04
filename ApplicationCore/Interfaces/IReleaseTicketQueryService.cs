using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IReleaseTicketQueryService : IRepository<ReleaseTicket>
{
    List<ReleaseTicket> GetTicketsByNumber(int quantity,int eventId,int ticketTypeId,int memberId);
    List<ReleaseTicket> GetAll(int userId);

    int GetTicketCount(int memberId,int eventId, int ticketTypeId);
    
    //查詢出該票券的活動主辦方
    int GetTicketHostId(string ticketNumbers);
    
    List<ReleaseTicketSummaryDTO> GetReleaseTicketSummaries(int userId,int page,int pageSize);

    List<UsedTicketsSummaryDTO> GetUsedTickets(int userId,int page,int pageSize);

    int GetTicketsCount(int userId);

    int GetUsedTicketsCount(int userId);
    
    //測試後端分頁
    Task<IPagedList<ReleaseTicketSummaryDTO>> GetReleaseTicketListByPageAsync(ReleaseTicketParameters releaseTicketParameters);
}