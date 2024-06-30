using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/order-items")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        public OrderItemsController(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository ?? throw new ArgumentNullException(nameof(orderItemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
        {
            var orderItemEntities = await _orderItemRepository.GetOrderItemsAsync();

            return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(orderItemEntities));
        }

        [HttpGet("{id}", Name = "GetOrderItem")]
        public async Task<ActionResult> GetOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(
           OrderItemForCreationDto orderItem)
        {
            var orderId = orderItem.OrderId;
            var menuItemId = orderItem.MenuItemId;
            if (!await _orderItemRepository.OrderAndItemExistsAsync(orderId, menuItemId))
            {
                return NotFound();
            }

            var finalOrderItem = _mapper.Map<OrderItem>(orderItem);

            await _orderItemRepository.CreateOrderItemAsync(
                orderId, menuItemId, finalOrderItem);

            await _orderItemRepository.SaveChangesAsync();

            var createdOrderItemToReturn =
                _mapper.Map<OrderItemSimpleDto>(finalOrderItem);

            return CreatedAtRoute("GetOrderItem",
                 new
                 {
                     id = createdOrderItemToReturn.OrderItemId
                 },
                 createdOrderItemToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderItem(int id,
         OrderItemForUpdateDto orderItem)
        {
            var orderId = orderItem.OrderId;
            var menuItemId = orderItem.MenuItemId;

            if (!await _orderItemRepository.OrderAndItemExistsAsync(orderId, menuItemId))
            {
                return NotFound();
            }

            var orderItemEntity = await _orderItemRepository
                .GetOrderItemAsync(id);
            if (orderItemEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(orderItem, orderItemEntity);

            await _orderItemRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderItem(int id)
        {
            var orderItemEntity = await _orderItemRepository
                .GetOrderItemAsync(id);
            if (orderItemEntity == null)
            {
                return NotFound();
            }

            _orderItemRepository.DeleteOrderItemAsync(orderItemEntity);
            await _orderItemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
