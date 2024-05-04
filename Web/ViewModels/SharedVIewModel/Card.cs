namespace Web.ViewModels.SharedVIewModel

{
    public class Card
    {
            public int EventId { get; set; }
            public bool IsLike { get; set; }
            public string EventCoverUrl { get; set; }
            public string EventTitle { get; set; }
            public DateTime EventStartDate { get; set; }
            public DateTime EventEndDate { get; set; }
            public string EventCity { get; set; }
            public int HeartCount { get; set; }
            public List<string> EventThemes { get; set; }
    }
}
