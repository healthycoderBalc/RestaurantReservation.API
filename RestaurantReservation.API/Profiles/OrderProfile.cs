using AutoMapper;
using RestaurantReservation.API.Models.Order;

namespace RestaurantReservation.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Db.Models.Order, OrderDto>();
            CreateMap<Db.Models.Order, OrderSimpleDto>();
            CreateMap<Db.Models.Order, OrderWithoutListsDto>();
            CreateMap<OrderForCreationDto, Db.Models.Order>();
            CreateMap<OrderForUpdateDto, Db.Models.Order>();
            CreateMap<Db.Models.Order, OrderForUpdateDto>();
            CreateMap<Db.Models.Order, OrderForReservationDto>();
        }
    }
}
