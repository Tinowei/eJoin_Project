using ApplicationCore.Interfaces;
using Infrastructure.Data;

namespace Admin.Configurations
{
    public static class ConfigureApplicationCoreService
    {
        /// <summary>
        /// 註冊ApplicationCore內相關的
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(EfUnitOfWork));

            return services;
        }
    }
}
