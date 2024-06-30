using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.API.Models.Reservation;

namespace RestaurantReservation.API.Models.Order
{
    public class OrderForReservationDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
