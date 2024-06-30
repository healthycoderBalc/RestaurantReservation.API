using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Employee
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();

        public ICollection<OrderSimpleDto> Orders { get; set; } = new List<OrderSimpleDto>();
    }
}
