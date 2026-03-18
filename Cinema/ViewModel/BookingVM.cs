using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.ViewModel
{
    public class BookingVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Select Movies")]
        [Display(Name ="Movies")]
        public int MovieId { get; set; }
        [Required(ErrorMessage = "Select Seat Type")]
        [Display(Name = "Seat Type")]
        public SeatType SeatType { get; set; }
        [Required(ErrorMessage = "Enter Number Of Tickets")]
        [Range(1, 10, ErrorMessage = "Number of tickets must be between 1 and 10.")]
        [Display(Name = "Number Of Tickets")]
        public int NumbetOFTikets { get; set; }
        [Required(ErrorMessage = "Select Show Time")]
        [Display(Name = "Show Time")]
        public DateTime ShowTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        //public IEnumerable<SelectListItem> Movies { get; set; } = null!;
        public IEnumerable<Movie> Movies { get; set; } = null!;

    }
}
