namespace RestaurantReservation.API.Models.OrderItem
{
    public class OrderItemForCreationDto
    {
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
    }
}
