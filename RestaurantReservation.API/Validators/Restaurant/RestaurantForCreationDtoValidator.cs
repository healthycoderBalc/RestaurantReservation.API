using FluentValidation;
using RestaurantReservation.API.Models.Restaurant;

namespace RestaurantReservation.API.Validators.Restaurant
{
    public class RestaurantForCreationDtoValidator : AbstractValidator<RestaurantForCreationDto>
    {
        public RestaurantForCreationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MinimumLength(5).WithMessage("Address must be at least 5 characters long.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches("^[0-9]+$").WithMessage("Phone Number must be numeric.");

            RuleFor(x => x.OpeningHours)
                .NotEmpty().WithMessage("Opening Hours are required.");
        }
    }
}
