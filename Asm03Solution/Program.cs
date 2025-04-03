using Asm03Solution.Components;
using BusinessObject.Services;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Asm03Solution.DataAccess.Repositories;
using DataAccess.Repositories;

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
            builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //cau hinh di repo+service
            builder.Services.AddScoped<MemberRepository>();
            builder.Services.AddScoped<MemberService>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<OrderDetailRepository>();
            builder.Services.AddScoped<OrderDetailService>();
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<ProductService>();

            //cau hinh session
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession(); // Enable session handling
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
