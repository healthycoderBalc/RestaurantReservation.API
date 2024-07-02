using FluentValidation;
using RestaurantReservation.API.Models.Customer;

namespace RestaurantReservation.API.Validators.Customer
{
    public class CustomerForUpdateDtoValidator : AbstractValidator<CustomerForUpdateDto>
    {
        public CustomerForUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .Matches("^[A-Za-z\\s]+$");

            RuleFor(x => x.LastName)
              .NotEmpty()
              .MinimumLength(2)
              .Matches("^[A-Za-z\\s]+$");

            RuleFor(x => x.Email)
              .NotEmpty()
              .WithMessage("Email should not be null")
              .EmailAddress()
              .WithMessage("A correct email address should be provided");

            RuleFor(x => x.PhoneNumber)
              .NotEmpty()
              .Matches("^[0-9]+$").WithMessage("Phone number must be numeric.");
        }
    }
}
