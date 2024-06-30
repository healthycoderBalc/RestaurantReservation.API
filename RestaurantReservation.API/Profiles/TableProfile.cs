using AutoMapper;
using RestaurantReservation.API.Models.Table;

namespace RestaurantReservation.API.Profiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Db.Models.Table, TableDto>();
            CreateMap<Db.Models.Table, TableSimpleDto>();
            CreateMap<Db.Models.Table, TableWithoutListsDto>();
            CreateMap<TableForCreationDto, Db.Models.Table>();
            CreateMap<TableForUpdateDto, Db.Models.Table>();
            CreateMap<Db.Models.Table, TableForUpdateDto>();
        }
    }
}
