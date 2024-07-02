using FluentValidation;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Validators.MenuItem
{
    public class MenuItemForUpdateDtoValidator : AbstractValidator<MenuItemForUpdateDto>
    {
        public MenuItemForUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required.")
               .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
               .MaximumLength(100).WithMessage("Name must be less than 100 characters long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Description must be less than 500 characters long.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .PrecisionScale(18, 2, false).WithMessage("Price must be a valid decimal with up to 2 decimal places.");
            RuleFor(x => x.RestaurantId)
             .NotEmpty().WithMessage("Restaurant ID is required.")
             .GreaterThan(0).WithMessage("Restaurant ID must be greater than 0.");
        }

    }
}
