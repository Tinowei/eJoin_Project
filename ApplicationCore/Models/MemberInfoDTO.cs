using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MemberInfoDTO
    {
        public int MemberId { get; set; }
        public string MemberAvatarUrl { get; set; }
        public string CoverImage { get; set; }
        public string MemberName { get; set; }
        public string MemberDescription { get; set; }
        public DateTime MemberBuildDate { get; set; }

        public int FansCount { get; set; }
        public int EventCount { get; set; }
        public List<string> MemberEventThemes { get; set; } = new List<string>();
        public List<PastEvent> PastEvents { get; set; }
        public List<FutureEvent> FutureEvents { get; set; }
    }

    public class PastEvent
    {
        public DateTime EventBeginDate { get; set; }
        public string PastEventImage { get; set; }
        public string EventName { get; set; }
        public string EventLocate { get; set; }
        public int EventId { get; set; }
    }

    public class FutureEvent
    {
        public string EventTitle { get; set; }
        public string EventCity { get; set; }
        public string EventConverUrl { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public List<string> EventThemes { get; set; }
        public int HeartCount { get; set; }
        public int EventId { get; set; }
    }
}