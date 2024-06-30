using AutoMapper;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.MenuItem;

namespace RestaurantReservation.API.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile() {
            CreateMap<Db.Models.MenuItem, MenuItemDto>();
            CreateMap<Db.Models.MenuItem, MenuItemWithoutListsDto>();
            CreateMap<Db.Models.MenuItem, MenuItemSimpleDto>();
            CreateMap<MenuItemForCreationDto, Db.Models.MenuItem>();
            CreateMap<MenuItemForUpdateDto, Db.Models.MenuItem>();
            CreateMap<Db.Models.MenuItem, MenuItemForUpdateDto>();
        }
    }
}
