using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/restaurants")]
    [Authorize]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        public RestaurantsController(IRestaurantRepository restaurantRespository, IMapper mapper)
        {
            _restaurantRepository = restaurantRespository ?? throw new ArgumentNullException(nameof(restaurantRespository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantWithoutListsDto>>> GetRestaurants()
        {
            var restaurantEntities = await _restaurantRepository.GetRestaurantsAsync();
            return Ok(_mapper.Map<IEnumerable<RestaurantWithoutListsDto>>(restaurantEntities));
        }

        [HttpGet("{id}", Name = "GetRestaurant")]
        public async Task<ActionResult> GetRestaurant(int id, bool includeLists = false)
        {
            var restaurant = await _restaurantRepository.GetRestaurantAsync(id, includeLists);
            if (restaurant == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<RestaurantDto>(restaurant));
            }
            return Ok(_mapper.Map<RestaurantWithoutListsDto>(restaurant));
        }

        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant(
     RestaurantForCreationDto restaurant)
        {
            var finalRestaurant = _mapper.Map<Restaurant>(restaurant);

            _restaurantRepository.CreateRestaurantAsync(
                finalRestaurant);

            await _restaurantRepository.SaveChangesAsync();

            var createdRestaurantToReturn =
                _mapper.Map<RestaurantWithoutListsDto>(finalRestaurant);

            return CreatedAtRoute("GetRestaurant",
                 new
                 {
                     id = createdRestaurantToReturn.RestaurantId
                 },
                 createdRestaurantToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRestaurant(int id,
         RestaurantForUpdateDto restaurant)
        {
            var restaurantEntity = await _restaurantRepository
                .GetRestaurantAsync(id, false);
            if (restaurantEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(restaurant, restaurantEntity);

            await _restaurantRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestaurant(int id)
        {
            var restaurantEntity = await _restaurantRepository
                .GetRestaurantAsync(id, false);
            if (restaurantEntity == null)
            {
                return NotFound();
            }

            _restaurantRepository.DeleteRestaurantAsync(restaurantEntity);
            await _restaurantRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{restaurantId}/total-revenue")]
        public async Task<ActionResult<decimal>> GetTotalRevenue(int restaurantId)
        {
            var totalRevenue = await _restaurantRepository.GetRestaurantTotalRevenueAsync(restaurantId);
            return Ok(totalRevenue);
        }
    }
}
