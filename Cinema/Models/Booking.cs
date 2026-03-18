using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    public enum SeatType
    {
        Regular,
        VIP
    }
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        [Required]
        [Display(Name = "Seat Type")]
        public SeatType SeatType { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Number of tickets must be between 1 and 10.")]
        [Display(Name = "Number of Tickets")]
        public int NumbetOFTikets { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        [Required]
        [Display(Name = "Show Time")]
        public DateTime ShowTime { get; set; }
    }
}
