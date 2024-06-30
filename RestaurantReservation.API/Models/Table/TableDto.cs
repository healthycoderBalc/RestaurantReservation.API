using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Models.Table
{
    public class TableDto
    {
        public int TableId { get; set; }
        public int Capacity { get; set; }

        public RestaurantWithoutListsDto Restaurant { get; set; } = new RestaurantWithoutListsDto();

        public ICollection<ReservationSimpleDto> Reservations { get; set; } = new List<ReservationSimpleDto>();
    }
}
