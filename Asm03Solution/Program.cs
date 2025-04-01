using Asm03Solution.Components;
using BusinessObject.Services;
using BusinessObject.Services.Instance;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Asm03Solution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
                
            // Cấu hình SignalR với thời gian chờ cao hơn
            builder.Services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                options.KeepAliveInterval = TimeSpan.FromSeconds(30);
            });
            
            // Thêm nén phản hồi để cải thiện hiệu suất
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            
            // Add controllers
            builder.Services.AddControllers();
            
            // Enable Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
                
            // Cấu hình Cookie Policy
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
                
            // Register DbContext
            builder.Services.AddDbContext<ECommerceContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                
            // Register Repositories and Unit of Work
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Register Services
            builder.Services.AddScoped<IMemberService, MemberService>();
            
            var app = builder.Build();

            // Initialize and seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    DbInitializer.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Sử dụng nén response
            app.UseResponseCompression();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAntiforgery();
            
            // Map controllers
            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
