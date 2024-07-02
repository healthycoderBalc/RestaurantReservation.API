using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.MenuItem
{
    /// <summary>
    /// Menu Item
    /// </summary>
    public class MenuItemWithoutListsDto
    {
        /// <summary>
        /// Id of menu item
        /// </summary>
        public int MenuItemId { get; set; }

        /// <summary>
        /// menu item name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of menu item
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        ///  Price of menu item
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// Restaurant where the menu item is ofered
        /// </summary>
        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();
    }
}
