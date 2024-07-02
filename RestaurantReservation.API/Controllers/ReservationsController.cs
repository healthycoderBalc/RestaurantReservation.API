using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using System.Runtime.InteropServices;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/reservations")]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<ReservationWithoutListsDto>>> GetReservations([FromQuery] bool withExtraInfo = false)
        {
            if (withExtraInfo)
            {
                var reservationEntitiesWithCustomerAndRestaurantInfo = await _reservationRepository.GetReservationsWithCustomerAndRestaurantInformationFromViewAsync();
                return Ok(_mapper.Map<IEnumerable<ReservationWithDetailsDto>>(reservationEntitiesWithCustomerAndRestaurantInfo));
            }
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
       [FromBody] ReservationForCreationDto reservation)
        {
            var customerId = reservation.CustomerId;
            var restaurantId = reservation.RestaurantId;
            var tableId = reservation.TableId;
            if (!await _reservationRepository.CustomerRestaurantAndTableExistsAsync(customerId, restaurantId, tableId))
            {
                return NotFound();
            }

            if (reservation == null)
            {
                return BadRequest();
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

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByCustomerId(int customerId)
        {
            if (!await _reservationRepository.CustomerExistAsync(customerId))
            {
                return NotFound();
            }

            var reservationEntities = await _reservationRepository.GetReservationsByCustomerAsync(customerId);

            return Ok(_mapper.Map<IEnumerable<ReservationWithoutListsDto>>(reservationEntities));
        }

        [HttpGet("{reservationId}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByReservation(int reservationId)
        {
            var orderEntities = await _reservationRepository.ListOrderAndMenuItemsAsync(reservationId);

            return Ok(_mapper.Map<IEnumerable<OrderForReservationDto>>(orderEntities));
        }

        [HttpGet("{reservationId}/menu-items")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetOrderedMenusByReservation(int reservationId)
        {
            var menuItemEntities = await _reservationRepository.ListOrderedMenuItemsAsync(reservationId);

            return Ok(_mapper.Map<IEnumerable<MenuItemSimpleDto>>(menuItemEntities));
        }

    }
}
