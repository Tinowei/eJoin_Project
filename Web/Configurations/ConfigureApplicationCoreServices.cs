using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;

namespace Web.Configurations
{
    public static class ConfigureApplicationCoreServices
    {
        public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IQrCodeHelper), typeof(QrCodeHelper));
            services.AddScoped(typeof(IEventRepository), typeof(EfEventRepository));
            services.AddScoped<IOrderRepository, EfOrderRepository>();
            services.AddScoped<IMemberRepository, EfMemberRepository>();
            services.AddScoped<IReleaseTicketQueryService, ReleaseTicketQueryService>();
            services.AddScoped(typeof(IReleaseTicketRepository), typeof(EfReleaseTicketRepository));
            services.AddScoped(typeof(IUnitOfWork), typeof(EfUnitOfWork));
            services.AddScoped <IOrderDetailQueryService,OrderDetailQueryService> ();
            services.AddScoped<ISendEmail, SendEmailMailKit>();
            services.AddScoped<ISearchQueryService,SearchQueryService>();
            return services;
        }
    }
}
