namespace Cinema.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        [Required]
        public string SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public int CinemaId { get; set; }
        public CinemaDeteils Cinema { get; set; }
        public Ticket Ticket { get; set; }
    }
}
