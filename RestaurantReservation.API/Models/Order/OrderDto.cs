using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.API.Models.Reservation;

namespace RestaurantReservation.API.Models.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ReservationSimpleDto Reservation { get; set; } = new ReservationSimpleDto();
        public EmployeeSimpleDto Employee { get; set; } = new EmployeeSimpleDto();

        public ICollection<OrderItemSimpleDto> OrderItems { get; set; } = new List<OrderItemSimpleDto>();
    }
}
