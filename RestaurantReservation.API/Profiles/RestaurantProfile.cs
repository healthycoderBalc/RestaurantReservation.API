using AutoMapper;
using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Profiles
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Db.Models.Restaurant, RestaurantDto>();
            CreateMap<Db.Models.Restaurant, RestaurantWithoutListsDto>();
            CreateMap<RestaurantForCreationDto, Db.Models.Restaurant>();
            CreateMap<RestaurantForUpdateDto, Db.Models.Restaurant>();
            CreateMap<Db.Models.Restaurant, RestaurantForUpdateDto>();
        }
    }
}
