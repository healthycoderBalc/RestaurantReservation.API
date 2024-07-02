using FluentValidation;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Validators.Employee
{
    public class EmployeeForUpdateDtoValidator : AbstractValidator<EmployeeForUpdateDto>
    {
        public EmployeeForUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName)
           .NotEmpty()
           .MinimumLength(2)
           .Matches("^[A-Za-z\\s]+$");

            RuleFor(x => x.LastName)
              .NotEmpty()
              .MinimumLength(2)
              .Matches("^[A-Za-z\\s]+$");

            RuleFor(x => x.Position)
              .NotEmpty();

            RuleFor(x => x.RestaurantId)
                       .NotEmpty().WithMessage("Restaurant ID is required.")
                       .GreaterThan(0).WithMessage("Restaurant ID must be greater than 0.");
        }
    }
}
