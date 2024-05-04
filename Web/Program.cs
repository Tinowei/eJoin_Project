using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;
using Web.Configurations;
using Web.WebApiServices.TicketServices;
using Web.WebApiServices.MemberServices;
using CloudinaryDotNet;


namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
            builder.Services.AddApplicationCoreServices().AddWebServices();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    //options.AccessDeniedPath = "/AccessDenied.html";
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
                })
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    options.SaveTokens = true;
                });


            builder.Services.AddSingleton<Cloudinary>(sp =>
            {
                var cloudName = builder.Configuration["Cloudinary:CloudName"];
                var apiKey = builder.Configuration["Cloudinary:ApiKey"];
                var apiSecret = builder.Configuration["Cloudinary:ApiSecret"];
                var account = new Account(cloudName, apiKey, apiSecret);
                return new Cloudinary(account);
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
                options.InstanceName = "MyRedisCache";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/Error/404");


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.Run();
        }
    }
}
