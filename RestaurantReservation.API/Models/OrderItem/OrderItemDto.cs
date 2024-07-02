using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Order;

namespace RestaurantReservation.API.Models.OrderItem
{
    /// <summary>
    /// Order Item
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// Id of order item
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// Quantity of order item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Order for order item
        /// </summary>
        public OrderSimpleDto Order { get; set; } = new OrderSimpleDto();

        /// <summary>
        /// menu item for order item
        /// </summary>
        public MenuItemSimpleDto MenuItem { get; set; } = new MenuItemSimpleDto();

    }
}
