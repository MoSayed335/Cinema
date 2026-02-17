using Microsoft.EntityFrameworkCore;
using Cinema.ViewModel;
namespace Cinema.DataAccess
{
    public class ApplicationDBContxet : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContxet(DbContextOptions<ApplicationDBContxet> options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaDeteils> Cinemas { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<ApplicationOTP> ApplicationOTPs { get; set; }
        public DbSet<Cinema.ViewModel.ResetPasswordVM> ResetPasswordVM { get; set; } = default!;
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=MARSNAN-UMMNUUB;Initial Catalog=CinemaSystem;Integrated Security=True;Connect Timeout=300;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}
