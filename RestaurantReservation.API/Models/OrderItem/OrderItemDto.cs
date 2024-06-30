using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Order;

namespace RestaurantReservation.API.Models.OrderItem
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        public OrderSimpleDto Order { get; set; } = new OrderSimpleDto();
        public MenuItemSimpleDto MenuItem { get; set; } = new MenuItemSimpleDto();

    }
}
