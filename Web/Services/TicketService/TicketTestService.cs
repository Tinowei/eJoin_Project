
using Web.ViewModels.TicketViewModel;

namespace Web.Services.TicketService
{
    public class TicketTestService
    {
        public IndexViewModel GetTicketViewModel()
        {
            return new IndexViewModel
            {         
                EventName="設計交流之夜：工藝師與建築師的空間打造",
                EventEndDate="2024-03-01 20:30",
                TicketNumber="2401310739245745410350",
                TicketType="一般票",
                ParticipantName="譚舒舒",
                ParticipantEmail="123456@gmail.com",
                ParticipantPhone="0909090090",
                QRcodeUrl="/images/QRcode.png",
                OrganizerName="工藝社",
                OrganizerEmail="520520@gmail.com",
                OrganizerPhone="0909520520",          
            };
        }

    }
}
