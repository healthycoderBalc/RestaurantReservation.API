using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.API.Models.Table;

namespace RestaurantReservation.API.Models.Reservation
{
    public class ReservationForUpdateDto
    {
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int TableId { get; set; }
    }
}
