using GymManagementDAL.Entities;
using GymmanagmentBLL;
using GymmanagmentBLL.Services.Classes;
using GymmanagmentBLL.Services.Interfaces;
using GymManagmentDAL.Data.Contexts;
using GymManagmentDAL.Data.DataSeed;
using GymManagmentDAL.Repositories.Classes;
using GymManagmentDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                Config.Password.RequiredLength = 6;
                Config.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
            var app = builder.Build();

            #region Data Seeding
            using var scope = app.Services.CreateScope();
            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            GymDataSeeding.SeedData(gymDbContext);
            IdentityDataSeeding.SeedData(roleManager, userManager);
            #endregion

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
