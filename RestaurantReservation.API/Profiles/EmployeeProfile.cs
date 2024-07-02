using AutoMapper;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Employee;

namespace RestaurantReservation.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Db.Models.Employee, EmployeeDto>();
            CreateMap<Db.Models.Employee, EmployeeSimpleDto>();
            CreateMap<Db.Models.Employee, EmployeeWithoutListsDto>();
            CreateMap<EmployeeForCreationDto, Db.Models.Employee>();
            CreateMap<EmployeeForUpdateDto, Db.Models.Employee>();
            CreateMap<Db.Models.Employee, EmployeeForUpdateDto>();
            CreateMap<Db.Models.EmployeeWithRestaurantDetails, EmployeeWithRestaurantDetailsDto>();
        }
    }
}
