using System.Configuration;
using Web.Services.AccountService;
using Web.Services.CacheService;
using Web.Services.HomeService;
using Web.Services.HostService;
using Web.Services.MemberService;
using Web.Services.OrganizeService;
using Web.Services.RegisterService;
using Web.WebApiServices.FollowServices;
using Web.WebApiServices.HostServices;
using Web.WebApiServices.LikesApiService;
using Web.WebApiServices.MemberServices;
using Web.WebApiServices.OrganizeServices;
using Web.WebApiServices.RegisterServices;
using Web.WebApiServices.SearchServices;
using Web.WebApiServices.SignupServices;
using Web.WebApiServices.TicketServices;
using Web.WebApiServices.UploadServices;

namespace Web.Configurations
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            // Service
            //services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IHomeService, RedisCacheHomeViewModelService>();
            services.AddScoped<HomeService>();
            services.AddScoped<MemberService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<RegisterService>();
            services.AddScoped<OrganizeService>();
            services.AddScoped<MemberService>();
            services.AddScoped<HostService>();
            services.AddScoped<GoogleAccountService>();

            // Api Service
            services.AddScoped<RegisterApiService>();
            services.AddScoped<TicketApiService>();
            services.AddScoped<OrganizeApiService>();
            services.AddScoped<MemberApiServices>();
            services.AddScoped<FollowApiService>();
            services.AddScoped<LikesApiService>();
            services.AddScoped<HostApiService>();
            services.AddScoped<SignupApiService>();
            services.AddScoped<SearchApiService>();

            services.AddHttpContextAccessor();

            // 要拿掉註解先找Adam services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserContextService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddScoped<UploadImageService>();

            services.AddScoped<InjectToViewService>();
            return services;
        }
    }
}
