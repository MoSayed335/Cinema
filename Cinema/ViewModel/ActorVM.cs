namespace Cinema.ViewModel
{
    public class ActorVM
    {
        public IEnumerable<Actor>? Actors { get; set; }
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
        public string? SearchName { get; set; }
    }
}
