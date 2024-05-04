using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class EfMemberRepository : EfRepository<Member> , IMemberRepository
    {
        public EfMemberRepository(EJoinContext dbContext) : base(dbContext)
        {
        }

        public MemberInfoDTO GetMemberInfoByMemberId(int memberId)
        {
            var member = DbContext.Members.FirstOrDefault(x => x.Id == memberId);

            if (member == null) throw new Exception("member is not found : " + memberId);

            var fansCount = DbContext.Follows.Count(x=> x.BeingFollowedId == memberId);

            var eventCount = DbContext.Events.Count(x=> x.MemberId == memberId);

            //取得會員的活動主題
            var memberEventThemes = 
                (from events in DbContext.Events where events.MemberId == memberId
                join eventThemes in DbContext.EventThemes
                    on events.Id equals eventThemes.EventId
                join themes in DbContext.Themes 
                    on eventThemes.ThemeId equals themes.Id
                group themes by themes.ThemeName into groupThemeName
                where groupThemeName.Key != null
                select groupThemeName.Key).ToList();

            var today = DateTime.Today;
            var oneMonthLater = today.AddMonths(1);

            var pastEvents =
                from events in DbContext.Events
                where events.MemberId == memberId &&
                    events.StartTime <= today &&
                    events.Status == 3
                select new PastEvent
                {
                    EventId = events.Id,
                    EventBeginDate = events.StartTime,
                    PastEventImage = events.CoverUrl,
                    EventName = events.Title,
                    EventLocate = events.City
                };

            //即將舉辦之活動
            var futureEvents = from events in DbContext.Events
                               where events.MemberId == memberId &&
                               events.Status == 2 &&
                               events.StartTime > today && events.StartTime <= oneMonthLater
                               select new FutureEvent
                               {
                                   EventId = events.Id,
                                   EventTitle = events.Title,
                                   EventCity = events.City,
                                   EventConverUrl = events.CoverUrl,
                                   EventStartDate = events.StartTime,
                                   EventEndDate = events.EndTime,
                                   EventThemes = (from eventThemes in DbContext.EventThemes
                                                      where eventThemes.EventId == events.Id
                                                      join themes in DbContext.Themes
                                                      on eventThemes.ThemeId equals themes.Id
                                                  select themes.ThemeName).ToList(),
                                   HeartCount = (from likes in DbContext.Likes
                                                    where likes.EventId == events.Id
                                                 select likes.Id).Count()

                               };

            return new MemberInfoDTO
            {
                MemberId = member.Id,
                MemberAvatarUrl = member.AvatarUrl ?? string.Empty,
                CoverImage = member.CoverUrl ?? string.Empty,
                MemberName = member.Name,
                MemberDescription = member.Description ?? string.Empty,
                MemberBuildDate = member.RegisterTime,
                FansCount = fansCount,
                EventCount = eventCount,
                MemberEventThemes = memberEventThemes,
                PastEvents = pastEvents.ToList(),
                FutureEvents = futureEvents.ToList(),
            };
        }
    }
}
