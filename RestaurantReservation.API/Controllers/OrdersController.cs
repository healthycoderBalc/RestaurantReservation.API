using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderWithoutListsDto>>> GetOrders()
        {
            var orderEntities = await _orderRepository.GetOrdersAsync();

            return Ok(_mapper.Map<IEnumerable<OrderWithoutListsDto>>(orderEntities));
        }

        [HttpGet("{id}", Name = "GetOrder")]
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

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(
          OrderForCreationDto order)
        {
            var reservationId = order.ReservationId;
            var employeeId = order.EmployeeId;
            if (!await _orderRepository.ReservationAndEmployeeExistsAsync(reservationId, employeeId))
            {
                return NotFound();
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
