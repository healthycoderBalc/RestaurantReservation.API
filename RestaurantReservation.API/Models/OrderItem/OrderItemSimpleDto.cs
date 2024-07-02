using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Order;

namespace RestaurantReservation.API.Models.OrderItem
{
    public class OrderItemSimpleDto
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int MenuItemId { get; set; }

    }
}
