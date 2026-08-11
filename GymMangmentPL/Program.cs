using GymMangmentBLL;
using GymMangmentDAL.Data.Contexts;
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

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if(pendingMigrations !=null && pendingMigrations.Any()) dbContext.Database.Migrate();
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
