namespace Cinema.DataAccess
{
    public class ApplicationDBContxet : DbContext
    {
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaDeteils> Cinemas { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=MARSNAN-UMMNUUB;Initial Catalog=CinemaSystem;Integrated Security=True;Connect Timeout=300;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Ticket>()
        //        .HasOne(t => t.Seat)
        //        .WithOne(s => s.Ticket)
        //        .HasForeignKey<Ticket>(t => t.SeatId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Ticket>()
        //        .HasOne(t => t.Movie)
        //        .WithMany(m => m.Tickets)
        //        .HasForeignKey(t => t.MovieId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Ticket>()
        //        .HasOne(t => t.User)
        //        .WithMany(u => u.Tickets)
        //        .HasForeignKey(t => t.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}

    }
}
