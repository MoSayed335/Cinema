using Cinema.Services.DbIntialize;
using Cinema.Utility;
using Stripe;
namespace Cinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDBContxet>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
                );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option=>
            {
                option.User.RequireUniqueEmail = false;
                option.SignIn.RequireConfirmedEmail= true;
                option.Password.RequiredLength = 8;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDBContxet>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<IRepository<CinemaDeteils>, Repository<CinemaDeteils>>();
            builder.Services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<Actor>, Repository<Actor>>();
            builder.Services.AddScoped<IRepository<ApplicationOTP>, Repository<ApplicationOTP>>();
            builder.Services.AddTransient<IEmailSender, EmailSend>();
            builder.Services.AddScoped<IRepository<Booking>, Repository<Booking>>();
            builder.Services.AddScoped<IDbintialize, Dbintialize>();
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            var scope = app.Services.CreateScope();
            var Service = scope.ServiceProvider.GetService<IDbintialize>();
            Service.Intializer();

            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Cinema}/{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
