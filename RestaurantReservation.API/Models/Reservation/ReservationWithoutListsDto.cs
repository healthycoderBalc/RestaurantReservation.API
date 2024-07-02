using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.API.Models.Table;

namespace RestaurantReservation.API.Models.Reservation
{
    /// <summary>
    /// reservation
    /// </summary>
    public class ReservationWithoutListsDto
    {
        /// <summary>
        /// id for reservation
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// reservation date
        /// </summary>
        public DateTime ReservationDate { get; set; }

        /// <summary>
        ///  Party size of reservation
        /// </summary>
        public int PartySize { get; set; }

        /// <summary>
        /// Customer who made the reservation
        /// </summary>
        public CustomerWithoutListsDto Customer { get; set; } = new CustomerWithoutListsDto();

        /// <summary>
        /// Restaurant associated to reservation
        /// </summary>
        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();

        /// <summary>
        /// reservation table
        /// </summary>
        public TableSimpleDto Table { get; set; } = new TableSimpleDto();
    }
}
