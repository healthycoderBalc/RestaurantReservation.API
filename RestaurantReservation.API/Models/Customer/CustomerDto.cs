using RestaurantReservation.API.Models.Reservation;

namespace RestaurantReservation.API.Models.Customer
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<ReservationSimpleDto> Reservations { get; set; } = new List<ReservationSimpleDto>();

    }
}
