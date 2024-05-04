namespace Web.WebApi.Register
{
    public class SelectTicketRequest
    {
        public int EventId { get; set; }
        public List<Ticket> Tickets { get; set; }
    }

    public class Ticket
    {
        public int TicketTypeId { get; set; }
        public int Count { get; set; }
    }

    public class SelectTicketResponse
    {
        public bool IsDone { get; set; }
        public int CartId { get; set; }

        public SelectTicketResponse()
        {
            IsDone = false;
            CartId = 0;
        }
    }
}
