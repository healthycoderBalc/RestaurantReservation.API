using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Employee
{
    /// <summary>
    /// An employee without associated lists
    /// </summary>
    public class EmployeeWithoutListsDto
    {
        /// <summary>
        /// Id of employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// First name of employee
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of employee
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// position of employee
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        ///  Restaurant where the employee works
        /// </summary>
        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();
    }
}
