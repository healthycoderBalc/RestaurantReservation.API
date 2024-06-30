using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.MenuItem
{
    public class MenuItemWithoutListsDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();
    }
}
