using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.Reservation;

namespace RestaurantReservation.API.Models.Order
{
    /// <summary>
    /// An order
    /// </summary>
    public class OrderWithoutListsDto
    {
        /// <summary>
        /// Id of order
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Date of order
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Total amount of the order
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Reservation for the order
        /// </summary>
        public ReservationSimpleDto Reservation { get; set; } = new ReservationSimpleDto();

        /// <summary>
        /// Employee for the order
        /// </summary>
        public EmployeeSimpleDto Employee { get; set; } = new EmployeeSimpleDto();
    }
}
