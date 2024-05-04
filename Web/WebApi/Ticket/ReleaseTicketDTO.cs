namespace Web.WebApi.Ticket
{
    // public class ReleaseTicketDTO
    // {
    //     public int EventId { get; set; }
    //     public DateTime ExpireTime { get; set; }
    //     
    //     public int TicketTypeId { get; set; }
    //     public string TicketTypeName { get; set; }
    //     public string ParticipantName { get; set; }
    //     public string ParticipantEmail { get; set; }
    //     public string ParticipantPhone { get; set; }
    // }

    public class ReleaseTicketSummaryDTO
    {
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireTime { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantEmail { get; set; }
        public string ParticipantPhone { get; set; }
    }
}
