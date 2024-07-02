using FluentValidation;
using RestaurantReservation.API.Models.Table;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Validators.Table
{
    public class TableForUpdateDtoValidator : AbstractValidator<TableForUpdateDto>
    {
        public TableForUpdateDtoValidator()
        {
            RuleFor(x => x.Capacity)
                   .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
                   .LessThanOrEqualTo(20).WithMessage("Capacity must be 20 or less.");


            RuleFor(x => x.RestaurantId)
                        .NotEmpty().WithMessage("Restaurant ID is required.")
                        .GreaterThan(0).WithMessage("Restaurant ID must be greater than 0.");
        }
    }
}
