namespace RestaurantReservation.API.Models.MenuItem
{
    public class MenuItemSimpleDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int RestaurantId { get; set; }
    }
}