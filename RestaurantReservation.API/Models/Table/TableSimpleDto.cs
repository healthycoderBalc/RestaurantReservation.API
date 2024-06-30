using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Table
{
    public class TableSimpleDto
    {
        public int TableId { get; set; }
        public int Capacity { get; set; }
        public int RestaurantId { get; set; }
    }
}
