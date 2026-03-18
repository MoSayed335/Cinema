namespace Cinema.ViewModel
{
    public class DashboardVM
    {
        public int TotalMovies { get; set; }
        public int TotalCinemas { get; set; }
        public int TotalBookings { get; set; }
        public string MostRecentMovie { get; set; } = string.Empty;
        public string LargestCinema { get; set; } = string.Empty;
        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
        public IEnumerable<Booking>? bookings { get; set; }
        public List<string>? LatestMovies { get; set; } 
    }
}
