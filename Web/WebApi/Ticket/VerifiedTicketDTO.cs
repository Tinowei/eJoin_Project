namespace Web.WebApi.Ticket;

public class VerifiedTicketDTO
{
    public string TicketNumber { get; set; }
    public int Status { get; set; }
    public DateTime ExpireTime { get; set; }
}