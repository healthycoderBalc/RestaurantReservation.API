using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.MenuItem
{
    public class MenuItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();

        public ICollection<OrderItemSimpleDto> OrderItems { get; set; } = new List<OrderItemSimpleDto>();
    }
}
