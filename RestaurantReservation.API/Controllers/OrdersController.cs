using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/orders")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all Orders
        /// </summary>
        /// <response code="201">Returns the orders</response>
        /// <returns>Orders</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<IEnumerable<OrderWithoutListsDto>>> GetOrders()
        {
            var orderEntities = await _orderRepository.GetOrdersAsync();

            return Ok(_mapper.Map<IEnumerable<OrderWithoutListsDto>>(orderEntities));
        }

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="includeLists">Whether or not to include the associated lists in the response</param>
        /// <response code="200">Returns the requested order</response>
        /// <returns>A order</returns>
        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OrderWithoutListsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetOrder(int id, bool includeLists = false)
        {
            var order = await _orderRepository.GetOrderAsync(id, includeLists);
            if (order == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<OrderDto>(order));

            }
            return Ok(_mapper.Map<OrderWithoutListsDto>(order));
        }

        /// <summary>
        /// Create an order
        /// </summary>
        /// <param name="order">order data for creation</param>
        /// <response code="200">Returns the created order</response>
        /// <returns>The created order</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> CreateOrder(
          [FromBody] OrderForCreationDto order)
        {
            var reservationId = order.ReservationId;
            var employeeId = order.EmployeeId;
            if (!await _orderRepository.ReservationAndEmployeeExistsAsync(reservationId, employeeId))
            {
                return NotFound();
            }
            if (order == null)
            {
                return BadRequest();
            }
            var finalOrder = _mapper.Map<Order>(order);

            await _orderRepository.CreateOrderAsync(
                reservationId, employeeId, finalOrder);

            await _orderRepository.SaveChangesAsync();

            var createdOrderToReturn =
                _mapper.Map<OrderWithoutListsDto>(finalOrder);

            return CreatedAtRoute("GetOrder",
                 new
                 {
                     id = createdOrderToReturn.OrderId
                 },
                 createdOrderToReturn);
        }

        /// <summary>
        /// Update order by id
        /// </summary>
        /// <param name="id">id of order</param>
        /// <param name="order">Order object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateOrder(int id,
         OrderForUpdateDto order)
        {
            var reservationId = order.ReservationId;
            var employeeId = order.EmployeeId;
            if (!await _orderRepository.ReservationAndEmployeeExistsAsync(reservationId, employeeId))
            {
                return NotFound();
            }

            var orderEntity = await _orderRepository
                .GetOrderAsync(id, false);
            if (orderEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(order, orderEntity);

            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="id">id of order</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var orderEntity = await _orderRepository
                .GetOrderAsync(id, false);
            if (orderEntity == null)
            {
                return NotFound();
            }

            _orderRepository.DeleteOrderAsync(orderEntity);
            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }



    }
}
