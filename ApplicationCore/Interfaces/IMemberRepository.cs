using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        MemberInfoDTO GetMemberInfoByMemberId(int memberId);
    }
}
