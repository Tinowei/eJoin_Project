using System;
using System.Collections.Generic;
using Web.ViewModels.SharedVIewModel;

namespace Web.ViewModels.HomeViewModel
{
#nullable disable
    public class IndexViewModel
    {

        public List<CarouselEvent> CarouselCards { get; set; }
        public List<Card> FeaturedCards { get; set; }
        public List<Card> HotCards { get; set; }
        public List<Card> HolidayCards { get; set; }

        //public List<Card> RecentlyViewed { get; set; }

    }
    public class CarouselEvent
    {
        public int EventId { get; set; }
        public string EventImgUrl { get; set; }
    }
}
//using System;
//using Web.ViewModels.MemberViewModel;

//namespace Web.ViewModels.HomeViewModel
//{
//    public class IndexViewModel
//    {
//        public List<HomeIndexCardGroupViewModel> IndexCard { get; set; }      

//    }

//    public class HomeIndexCardGroupViewModel
//    {
//        public List<HomeIndexCarouselImageGroupViewModel> CarouselImage { get; set; }
//        public string EventImage { get; set; }
//        public DateTime EventDateStart { get; set; }
//        public DateTime EventDateEnd { get; set; }
//        public string EventName { get; set; }
//        public string EventLocate { get; set; }
//        public string CardTag { get; set; }
//        public int LikeCount { get; set; }
//    }

//    public class HomeIndexCarouselImageGroupViewModel
//    {
//        public string RecommendedEvents { get; set;}

//        public string RecommendedEventImages { get; set; }
//    }
//}
