using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces;

public interface IReleaseTicketRepository : IRepository<ReleaseTicket>
{
    ReleaseTicket VerifyTicket(string StringTicketNumber);
    Task<(List<ReleaseTicket>, List<string> ErrorMessages)> VerifyTickets(List<string> ticketNumbers);
}