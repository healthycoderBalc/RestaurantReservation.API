namespace RestaurantReservation.API.Models.OrderItem
{
    public class OrderItemForUpdateDto
    {
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
    }
}
