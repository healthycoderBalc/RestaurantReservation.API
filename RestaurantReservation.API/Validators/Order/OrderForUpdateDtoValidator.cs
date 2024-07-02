using FluentValidation;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Validators.Order
{
    public class OrderForUpdateDtoValidator : AbstractValidator<OrderForUpdateDto>
    {
        public OrderForUpdateDtoValidator()
        {
            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than 0.")
                .PrecisionScale(18, 2, false).WithMessage("Total amount must be a valid decimal with up to 2 decimal places.");

            RuleFor(x => x.ReservationId)
                       .NotEmpty().WithMessage("Reservation ID is required.")
                       .GreaterThan(0).WithMessage("Reservation ID must be greater than 0.");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required.")
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0.");
        }
    }
}
