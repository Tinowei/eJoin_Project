using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using Web.ViewModels.HomeViewModel;
using Web.ViewModels.SharedVIewModel;

namespace Web.ViewModels.MemberViewModel
{
    public class IndexViewModel
    {
        public int MemberId { get; set; }
        public bool IsFollow { get; set; }
        public string CoverImage { get; set; }
        public string MemberImage { get; set; }
        public string MemberName { get; set; }
        public int FansCount { get; set; }
        public int EventCount { get; set; }
        public string MemberDescription { get; set; }
        public DateTime MemberBuildDate { get; set; }
        public List<string> MemberEventThemes { get; set; }
        public List<Card> PersonalCard { get; set; }
        public List<PastEventYear> PastEventByYear { get; set; }
        public int UserId { get; internal set; }
    }

    public class PastEventYear
    {
        public int EventYear { get; set; }
        public List<PastEvent> Events { get; set; }
    }

    public class PastEvent
    {
        public DateTime EventBeginDate { get; set; }
        public string EventBeginMonth
        {
            get
            {
                var month = EventBeginDate.Month;
                var monthEnglish = string.Empty;

                switch (month)
                {
                    case 1:
                        monthEnglish = "Jan";
                        break;

                    case 2:
                        monthEnglish = "Feb";
                        break;

                    case 3:
                        monthEnglish = "Mar";
                        break;

                    case 4:
                        monthEnglish = "Apr";
                        break;

                    case 5:
                        monthEnglish = "May";
                        break;

                    case 6:
                        monthEnglish = "Jun";
                        break;

                    case 7:
                        monthEnglish = "Jul";
                        break;

                    case 8:
                        monthEnglish = "Aug";
                        break;

                    case 9:
                        monthEnglish = "Sep";
                        break;

                    case 10:
                        monthEnglish = "Oct";
                        break;

                    case 11:
                        monthEnglish = "Nov";
                        break;

                    case 12:
                        monthEnglish = "Dec";
                        break;
                }


                return monthEnglish;
            }
        }
        public string PastEventImage { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventLocate { get; set; }
        public bool IsShowDate { get; internal set; }
    }
}
