namespace Cinema.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int SeatId { get; set; }
        public DateTime BookingDate { get; set; }
        public User User { get; set; }
        public Movie Movie { get; set; }
        public Seat Seat { get; set; }
    }
}
