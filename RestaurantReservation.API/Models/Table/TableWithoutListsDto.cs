using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Table
{
    public class TableWithoutListsDto
    {
        public int TableId { get; set; }
        public int Capacity { get; set; }
        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();
    }
}
