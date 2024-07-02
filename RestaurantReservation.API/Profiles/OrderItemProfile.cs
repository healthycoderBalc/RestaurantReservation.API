using AutoMapper;
using RestaurantReservation.API.Models.OrderItem;

namespace RestaurantReservation.API.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<Db.Models.OrderItem, OrderItemDto>();
            CreateMap<Db.Models.OrderItem, OrderItemSimpleDto>();
            CreateMap<OrderItemForCreationDto, Db.Models.OrderItem>();
            CreateMap<OrderItemForUpdateDto, Db.Models.OrderItem>();
            CreateMap<Db.Models.OrderItem, OrderItemForUpdateDto>();


        }
    }
}
