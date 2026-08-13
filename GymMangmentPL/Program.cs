using GymMangmentBLL;
using GymMangmentBLL.Services.Classes;
using GymMangmentBLL.Services.InterFaces;
using GymMangmentDAL.Data.Contexts;
using GymMangmentDAL.Data.DataSeeding;
using GymMangmentDAL.Repositories.Classes;
using GymMangmentDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymMangmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddScoped<ISessionRepository,SessionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            // servece Conctroll

            builder.Services.AddScoped<IAnalysisService, AnalysisService>();
            builder.Services.AddScoped<IMemberService, MemberService>();

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if(pendingMigrations !=null && pendingMigrations.Any()) dbContext.Database.Migrate();
            GymDbContextSeeding.SeedData(dbContext);
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
