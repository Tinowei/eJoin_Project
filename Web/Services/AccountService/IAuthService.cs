using ApplicationCore.Entities;
using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.Services.AccountService
{
    public interface IAuthService
    {
        Task<Member> AuthenticateUser(string email, string password);
        Task Login(Member user);
        Task<Member> RegisterLocalLoginAsync(string fullname, string email, string phone, string password);
    }
}
