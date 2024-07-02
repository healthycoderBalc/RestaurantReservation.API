using FluentValidation;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Validators.Reservation
{
    public class ReservationForCreationDtoValidator : AbstractValidator<ReservationForCreationDto>
    {
        public ReservationForCreationDtoValidator()
        {
            RuleFor(x => x.ReservationDate)
                .NotEmpty().WithMessage("Reservation Date is required.")
                .Must(BeAValidDate).WithMessage("Reservation Date must be a valid date.");

            RuleFor(x => x.PartySize)
                .GreaterThan(0).WithMessage("Party Size must be greater than 0.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.")
                .GreaterThan(0).WithMessage("Customer ID must be greater than 0.");

            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID is required.")
                .GreaterThan(0).WithMessage("Restaurant ID must be greater than 0.");

            RuleFor(x => x.TableId)
                .NotEmpty().WithMessage("Table ID is required.")
                .GreaterThan(0).WithMessage("Table ID must be greater than 0.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
