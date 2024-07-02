using FluentValidation;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Validators.OrderItem
{
    public class OrderItemForUpdateDtoValidator : AbstractValidator<OrderItemForUpdateDto>
    {
        public OrderItemForUpdateDtoValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required.")
                .GreaterThan(0).WithMessage("Order ID must be greater than 0.");

            RuleFor(x => x.MenuItemId)
                .NotEmpty().WithMessage("Menu Item ID is required.")
                .GreaterThan(0).WithMessage("Menu Item ID must be greater than 0.");
        }

    }
}
