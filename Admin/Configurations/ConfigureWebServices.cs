using Admin.Services;
using Admin.Services.SchedulerService;
using Coravel;
using Admin.Models.Settings;
using Microsoft.Extensions.Options;

namespace Admin.Configurations
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<UserMangerService>();
            services.AddScoped<OrderService>();
            services.AddScoped<EventService>();
            services.AddScoped<DashboardService>();

            //排程
            services.AddScheduler();
            services.AddTransient<EventSchedulerService>();
            services.AddTransient<DeleteCartSchedulerService>();
            services.AddTransient<OrderScheldulerService>();
            
            
            //lineBot
            services.AddHttpClient();
            services.Configure<OpenAISettings>(configuration.GetSection(OpenAISettings.SettingKey))
                .AddSingleton(setting => setting.GetRequiredService<IOptions<OpenAISettings>>().Value);
            services.Configure<LineBotSettings>(configuration.GetSection(LineBotSettings.SettingKey))
                .AddSingleton(setting => setting.GetRequiredService<IOptions<LineBotSettings>>().Value);

            services.AddScoped<EventConsultantService>();

            return services;
        }
    }
}
