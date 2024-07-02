using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
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

        /// <summary>
        /// Get Restaurants
        /// </summary>
        /// <response code="200">Returns the restaurants</response>
        /// <returns>Restaurants</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RestaurantWithoutListsDto>>> GetRestaurants()
        {
            var restaurantEntities = await _restaurantRepository.GetRestaurantsAsync();
            return Ok(_mapper.Map<IEnumerable<RestaurantWithoutListsDto>>(restaurantEntities));
        }

        /// <summary>
        /// Get restaurant by id
        /// </summary>
        /// <param name="id">restaurant id</param>
        /// <param name="includeLists">Whether or not to include associated lists</param>
        /// <response code="200">Returns the requested restaurant</response>
        /// <returns>A restaurant</returns>
        [HttpGet("{id}", Name = "GetRestaurant")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RestaurantWithoutListsDto), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Create a restaurant
        /// </summary>
        /// <param name="restaurant">Restaurant data for creation</param>
        /// <response code="201">Returns the created restaurant</response>
        /// <returns>The created restaurant</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant(
     [FromBody] RestaurantForCreationDto restaurant)
        {
            if (restaurant == null)
            {
                return BadRequest();
            }
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

        /// <summary>
        /// Update a restaurant by id
        /// </summary>
        /// <param name="id">restaurant id</param>
        /// <param name="restaurant">restaurant object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Delete restaurant by id
        /// </summary>
        /// <param name="id">Restaurant id</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Get total revenue for a restaurant
        /// </summary>
        /// <param name="restaurantId">restaurant id</param>
        /// <response code="200">Returns total revenue</response>
        /// <returns>total revenue</returns>
        [HttpGet("{restaurantId}/total-revenue")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(decimal),StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetTotalRevenue(int restaurantId)
        {
            var totalRevenue = await _restaurantRepository.GetRestaurantTotalRevenueAsync(restaurantId);
            return Ok(totalRevenue);
        }
    }
}
