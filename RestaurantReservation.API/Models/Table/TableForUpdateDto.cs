using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Table
{
    public class TableForUpdateDto
    {
        public int Capacity { get; set; }
        public int RestaurantId { get; set; }
    }
}
