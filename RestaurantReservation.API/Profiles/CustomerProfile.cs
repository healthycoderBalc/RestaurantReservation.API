using AutoMapper;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles
{
    public class CustomerProfile: Profile
    {
        public CustomerProfile()
        {
            CreateMap<Db.Models.Customer, CustomerWithoutListsDto>();
            CreateMap<Db.Models.Customer, CustomerDto>();
            CreateMap<CustomerForCreationDto, Db.Models.Customer>();
            CreateMap<CustomerForUpdateDto, Db.Models.Customer>();
            CreateMap<Db.Models.Customer, CustomerForUpdateDto>();
        }
    }
}
