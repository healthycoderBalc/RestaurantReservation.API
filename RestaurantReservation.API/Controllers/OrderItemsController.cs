using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/order-items")]
    [Authorize]
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

        /// <summary>
        /// Get all Order Items
        /// </summary>
        /// <response code="200">Returns the order items</response>
        /// <returns>Order Items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
        {
            var orderItemEntities = await _orderItemRepository.GetOrderItemsAsync();

            return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(orderItemEntities));
        }

        /// <summary>
        /// Get Order Item by id
        /// </summary>
        /// <param name="id">order item id</param>
        /// <response code="200">Returns the requested order item</response>
        /// <returns>A order item</returns>
        [HttpGet("{id}", Name = "GetOrderItem")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        /// <summary>
        /// Create an order item
        /// </summary>
        /// <param name="orderItem">order item data for creation</param>
        /// <response code="201">Returns the created order item</response>
        /// <returns>The created order item</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(
           [FromBody] OrderItemForCreationDto orderItem)
        {
            var orderId = orderItem.OrderId;
            var menuItemId = orderItem.MenuItemId;
            if (!await _orderItemRepository.OrderAndItemExistsAsync(orderId, menuItemId))
            {
                return NotFound();
            }

            if (orderItem == null)
            {
                return BadRequest();
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

        /// <summary>
        ///  Update Order Item by id
        /// </summary>
        /// <param name="id">id of order item</param>
        /// <param name="orderItem">Order item object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Delete Order item by id
        /// </summary>
        /// <param name="id">id of order item</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
