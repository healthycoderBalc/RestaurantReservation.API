using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.Models.Table;

namespace RestaurantReservation.API.Models.Restaurant
{
    public class RestaurantForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string OpeningHours { get; set; } = string.Empty;
    }
}
