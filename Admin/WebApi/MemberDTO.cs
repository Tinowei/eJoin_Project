namespace Web.WebApi.Member
{
    public class MemberDTO
    {
             
    }
    
    public class MemberData
    {
        public int MemberId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime? LastEditTime { get; set; }

    }


}
