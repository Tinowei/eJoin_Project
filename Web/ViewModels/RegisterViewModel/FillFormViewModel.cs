using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels.RegisterViewModel;
using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.ViewModels.RegisterViewModel
{
    public class FillFormViewModel
    {
        public int EventId { get; set; }

        public int CartId { get; set; }
        public string EventName { get; set; }

        public string EventPictureUrl { get; set; }

        public string EventAddress { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "最少需二個字")]
        public string ParticipantName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 10)] // 規定長度
        public string Email { get; set; }

        [RegularExpression(@"/^09\d{8}$/", ErrorMessage = "需為09開頭並且十個數字")]
        public string PhoneNumber { get; set; }

        public string LearnFrom { get; set; }
    }
}
