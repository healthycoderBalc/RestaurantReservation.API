using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationRepository reservationRepository, IMapper mapper )
        {
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationWithoutListsDto>>> GetReservations()
        {
            var reservationEntities = await _reservationRepository.GetReservationsAsync();

            return Ok(_mapper.Map<IEnumerable<ReservationWithoutListsDto>>(reservationEntities));
        }

        [HttpGet("{id}", Name = "GetReservation")]
        public async Task<ActionResult> GetReservation(int id, bool includeLists = false)
        {
            var reservation = await _reservationRepository.GetReservationAsync(id, includeLists);
            if (reservation == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<ReservationDto>(reservation));

            }
            return Ok(_mapper.Map<ReservationWithoutListsDto>(reservation));
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(
       ReservationForCreationDto reservation)
        {
            var customerId = reservation.CustomerId;
            var restaurantId = reservation.RestaurantId;
            var tableId = reservation.TableId;
            if (!await _reservationRepository.CustomerRestaurantAndTableExistsAsync(customerId, restaurantId, tableId))
            {
                return NotFound();
            }

            var finalReservation = _mapper.Map<Reservation>(reservation);

            await _reservationRepository.CreateReservationAsync(
                customerId, restaurantId, tableId, finalReservation);

            await _reservationRepository.SaveChangesAsync();

            var createdReservationToReturn =
                _mapper.Map<ReservationWithoutListsDto>(finalReservation);

            return CreatedAtRoute("GetReservation",
                 new
                 {
                     id = createdReservationToReturn.ReservationId
                 },
                 createdReservationToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReservation(int id,
         ReservationForUpdateDto reservation)
        {
            var customerId = reservation.CustomerId;
            var restaurantId = reservation.RestaurantId;
            var tableId = reservation.TableId;
            if (!await _reservationRepository.CustomerRestaurantAndTableExistsAsync(customerId, restaurantId, tableId))
            {
                return NotFound();
            }

            var reservationEntity = await _reservationRepository
                .GetReservationAsync(id, false);
            if (reservationEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(reservation, reservationEntity);

            await _reservationRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            var reservationEntity = await _reservationRepository
                .GetReservationAsync(id, false);
            if (reservationEntity == null)
            {
                return NotFound();
            }

            _reservationRepository.DeleteReservationAsync(reservationEntity);
            await _reservationRepository.SaveChangesAsync();

            return NoContent();
        }


    }
}
