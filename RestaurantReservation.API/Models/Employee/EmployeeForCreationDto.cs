using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Employee
{
    public class EmployeeForCreationDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
    }
}
