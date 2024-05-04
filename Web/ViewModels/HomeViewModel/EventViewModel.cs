
using Web.ViewModels.SharedVIewModel;

namespace Web.ViewModels.HomeViewModel
{
    public class EventViewModel
    {
        public List<Card> PreferredLikeEvents { get; set; }
        public int EventId { get; set; }
        public bool IsLike { get; set; }
        public int HeartCount { get; set; }
        public int MemberID { get; set; }
        public string EventCoverUrl { get; set; }
        public List<string> EventThemes { get; set; }
        public string MemberName { get; set; }
        public string MemberImages { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string AddressDetail { get; set; }
        public string EventSummary { get; set; }
        public string EventIntroduction { get; set; }
        public byte? EventStatus { get; internal set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
