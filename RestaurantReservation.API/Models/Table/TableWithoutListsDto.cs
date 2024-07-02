using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Table
{
    /// <summary>
    /// Table
    /// </summary>
    public class TableWithoutListsDto
    {
        /// <summary>
        /// Id of table
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Capacity of table
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Restaurant where the table is
        /// </summary>
        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();
    }
}
