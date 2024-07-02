using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/menu-items")]
    [Authorize]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;
        public MenuItemsController(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository ?? throw new ArgumentNullException(nameof(menuItemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all Menu Items
        /// </summary>
        /// <response code="200">Returns the menu items</response>
        /// <returns>Menu Items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItemWithoutListsDto>>> GetMenuItems()
        {
            var menuItemEntities = await _menuItemRepository.GetMenuItemsAsync();

            return Ok(_mapper.Map<IEnumerable<MenuItemWithoutListsDto>>(menuItemEntities));
        }

        /// <summary>
        /// Get Menu Item by id
        /// </summary>
        /// <param name="id">Id of the menu item to get</param>
        /// <param name="includeLists">whether or not to include associated lists</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMenuItem")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MenuItemWithoutListsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetMenuItem(int id, bool includeLists = false)
        {
            var menuItem = await _menuItemRepository.GetMenuItemAsync(id, includeLists);
            if (menuItem == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<MenuItemDto>(menuItem));
            }
            return Ok(_mapper.Map<MenuItemWithoutListsDto>(menuItem));
        }

        /// <summary>
        ///  Create a menu item
        /// </summary>
        /// <param name="menuItem"> Menu Item data for creation</param>
        /// <response code="201">Returns the created menu item</response>
        /// <returns>The created menu item</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<MenuItemDto>> CreateMenuItem(
        [FromBody] MenuItemForCreationDto menuItem)
        {
            var restaurantId = menuItem.RestaurantId;
            if (!await _menuItemRepository.RestaurantExistsAsync(menuItem.RestaurantId))
            {
                return NotFound("Invalid restaurant ID");
            }

            if (menuItem == null)
            {
                return BadRequest();
            }

            var finalMenuItem = _mapper.Map<MenuItem>(menuItem);

            await _menuItemRepository.CreateMenuItemAsync(
                restaurantId, finalMenuItem);

            await _menuItemRepository.SaveChangesAsync();

            var createdMenuItemToReturn =
                _mapper.Map<MenuItemWithoutListsDto>(finalMenuItem);

            return CreatedAtRoute("GetMenuItem",
                 new
                 {
                     id = createdMenuItemToReturn.MenuItemId
                 },
                 createdMenuItemToReturn);
        }

        /// <summary>
        /// Update a menu item by id
        /// </summary>
        /// <param name="id">id of the menu item</param>
        /// <param name="menuItem">Menu item object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateMenuItem(int id,
         MenuItemForUpdateDto menuItem)
        {
            var restaurantId = menuItem.RestaurantId;
            if (!await _menuItemRepository.RestaurantExistsAsync(restaurantId))
            {

                return NotFound();
            }

            var menuItemEntity = await _menuItemRepository
                .GetMenuItemAsync(id, false);
            if (menuItemEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(menuItem, menuItemEntity);

            await _menuItemRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete menu item by id
        /// </summary>
        /// <param name="id">id of menu item for deletion</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            var menuItemEntity = await _menuItemRepository
                .GetMenuItemAsync(id, false);
            if (menuItemEntity == null)
            {
                return NotFound();
            }

            _menuItemRepository.DeleteMenuItemAsync(menuItemEntity);
            await _menuItemRepository.SaveChangesAsync();

            return NoContent();
        }


    }
}
