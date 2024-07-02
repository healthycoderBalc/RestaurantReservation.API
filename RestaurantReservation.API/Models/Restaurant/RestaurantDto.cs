using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.Models.Table;

namespace RestaurantReservation.API.Models.Restaurant
{
    public class RestaurantDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string OpeningHours { get; set; } = string.Empty;

        public ICollection<TableSimpleDto> Tables { get; set; } = new List<TableSimpleDto>();

        public ICollection<EmployeeSimpleDto> Employees { get; set; } = new List<EmployeeSimpleDto>();

        public ICollection<MenuItemSimpleDto> MenuItems { get; set; } = new List<MenuItemSimpleDto>();

        public ICollection<ReservationSimpleDto> Reservations { get; set; } = new List<ReservationSimpleDto>();
    }
}
