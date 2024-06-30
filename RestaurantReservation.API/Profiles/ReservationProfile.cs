using AutoMapper;
using RestaurantReservation.API.Models.Reservation;

namespace RestaurantReservation.API.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Db.Models.Reservation, ReservationWithoutListsDto>();
            CreateMap<Db.Models.Reservation, ReservationDto>();
            CreateMap<Db.Models.Reservation, ReservationSimpleDto>();
            CreateMap<ReservationForCreationDto, Db.Models.Reservation>();
            CreateMap<ReservationForUpdateDto, Db.Models.Reservation>();
            CreateMap<Db.Models.Reservation, ReservationForUpdateDto>();
        }
    }
}
