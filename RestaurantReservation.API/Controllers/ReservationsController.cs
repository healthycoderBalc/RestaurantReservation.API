using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
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

        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <param name="withExtraInfo">Weather or not to include extra information</param>
        /// <response code="200">Returns the Reservations</response>
        /// <returns>Reservations</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Get a reservation by id
        /// </summary>
        /// <param name="id">reservation id</param>
        /// <param name="includeLists">Whether or not to include associated lists</param>
        /// <response code="200">Returns the requested reservation</response>
        /// <returns>A reservation</returns>
        [HttpGet("{id}", Name = "GetReservation")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReservationWithoutListsDto), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Create a reservation
        /// </summary>
        /// <param name="reservation">reservation data for creation</param>
        /// <response code="201">Returns the created customer</response>
        /// <returns>The created customer</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Update a reservation by id
        /// </summary>
        /// <param name="id">reservation id</param>
        /// <param name="reservation">Reservation object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Delete a reservation by id
        /// </summary>
        /// <param name="id">reservation id</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Get reservations by customer id
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <response code="200">Returns the reservations for that customer</response>
        /// <returns>Reservations</returns>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByCustomerId(int customerId)
        {
            if (!await _reservationRepository.CustomerExistAsync(customerId))
            {
                return NotFound();
            }

            var reservationEntities = await _reservationRepository.GetReservationsByCustomerAsync(customerId);

            return Ok(_mapper.Map<IEnumerable<ReservationWithoutListsDto>>(reservationEntities));
        }

        /// <summary>
        /// Get orders by reservation id
        /// </summary>
        /// <param name="reservationId">reservation id</param>
        /// <response code="200">Returns the orders for a reservation</response>
        /// <returns>Orders</returns>
        [HttpGet("{reservationId}/orders")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByReservation(int reservationId)
        {
            var orderEntities = await _reservationRepository.ListOrderAndMenuItemsAsync(reservationId);

            return Ok(_mapper.Map<IEnumerable<OrderForReservationDto>>(orderEntities));
        }

        /// <summary>
        ///  Get Ordered Menues by reservation id
        /// </summary>
        /// <param name="reservationId">reservation id</param>
        /// <response code="200">Returns the menu items for a reservation</response>
        /// <returns>Menu Items</returns>
        [HttpGet("{reservationId}/menu-items")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetOrderedMenusByReservation(int reservationId)
        {
            var menuItemEntities = await _reservationRepository.ListOrderedMenuItemsAsync(reservationId);

            return Ok(_mapper.Map<IEnumerable<MenuItemSimpleDto>>(menuItemEntities));
        }

    }
}
