using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Table
{
    public class TableForCreationDto
    {
        public int Capacity { get; set; }
        public int RestaurantId { get; set; }
    }
}
