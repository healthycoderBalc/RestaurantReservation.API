namespace RestaurantReservation.API.Models.Reservation
{
    public class ReservationWithDetailsDto
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
    }
}
