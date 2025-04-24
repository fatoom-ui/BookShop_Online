using BookShop.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        SeedData.Initialize(services);
            //    }
            //    catch(Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger>();
            //        logger.LogError(ex, message: "an error occuered when submit the data");
                              
            //    }
            //}
            //host.Run();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();


            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.
                Configuration.GetConnectionString("Connection")));

            //builder.Services.AddDefaultIdentity<DefaultUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddDefaultIdentity<DefaultUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            //builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddSession(options =>
            {

                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromSeconds(value:10);

            });
            builder.Services.AddScoped<Cart>();

            builder.Services.AddScoped<Cart>(sp =>Cart.GetCart(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Store}/{action=Index}/{id?}");

           
           

            SeedData.Initialize(app);
            //UseRolesIntializer.intializeAsync(services).Wait();

            // Create scope for services
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    UseRolesIntializer.intializeAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during role initialization.");
                }
            }



            app.Run();
        }
    }
}
