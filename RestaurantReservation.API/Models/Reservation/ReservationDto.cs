using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.API.Models.Table;

namespace RestaurantReservation.API.Models.Reservation
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }

        public CustomerWithoutListsDto Customer { get; set; } = new CustomerWithoutListsDto();
        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();
        public TableSimpleDto Table { get; set; } = new TableSimpleDto();

        public ICollection<OrderSimpleDto> Orders { get; set; } = new List<OrderSimpleDto>();
    }
}
