using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/menu-items")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemWithoutListsDto>>> GetMenuItems()
        {
            var menuItemEntities = await _menuItemRepository.GetMenuItemsAsync();

            return Ok(_mapper.Map<IEnumerable<MenuItemWithoutListsDto>>(menuItemEntities));
        }

        [HttpGet("{id}", Name = "GetMenuItem")]
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

        [HttpPost]
        public async Task<ActionResult<MenuItemDto>> CreateMenuItem(
        MenuItemForCreationDto menuItem)
        {
            var restaurantId = menuItem.RestaurantId;
            if (!await _menuItemRepository.RestaurantExistsAsync(menuItem.RestaurantId))
            {
                return NotFound();
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
