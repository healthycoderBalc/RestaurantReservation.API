using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    /// <summary>
    ///  Customer endpoints
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        /// <summary>
        ///  Get all customers
        /// </summary>
        /// <param name="partySize">When partySize provided, get customers with party size greater than value provided</param>
        /// <response code="200">Returns the customers</response>
        /// <returns>Customers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerWithoutListsDto>>> GetCustomers([FromQuery] int partySize)
        {
            if (partySize > 0)
            {
                var customerEntitiesWithPartySize = await _customerRepository.CustomerWithPartySizeGreaterThanAsync(partySize);
                return Ok(_mapper.Map<IEnumerable<CustomerWithoutListsDto>>(customerEntitiesWithPartySize));
            }
            var customerEntities = await _customerRepository.GetCustomersAsync();

            return Ok(_mapper.Map<IEnumerable<CustomerWithoutListsDto>>(customerEntities));
        }

        /// <summary>
        /// Get a customer by id
        /// </summary>
        /// <param name="id">The id of the customer to get</param>
        /// <param name="includeLists">Whether or not to include the associated lists in the response</param>
        /// <response code="200">Returns the requested customer</response>
        /// <returns>A customer</returns>
        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomerWithoutListsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCustomer(int id, bool includeLists = false)
        {
            var customer = await _customerRepository.GetCustomerAsync(id, includeLists);
            if (customer == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<CustomerDto>(customer));

            }
            return Ok(_mapper.Map<CustomerWithoutListsDto>(customer));
        }

        /// <summary>
        ///  Create a customer
        /// </summary>
        /// <param name="customer">Customer data for creation</param>
        /// <response code="201">Returns the created customer</response>
        /// <returns>The created customer</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CustomerWithoutListsDto>> CreateCustomer([FromBody] CustomerForCreationDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer cannot be null");
            }
            var finalCustomer = _mapper.Map<Customer>(customer);
            _customerRepository.CreateCustomerAsync(finalCustomer);
            await _customerRepository.SaveChangesAsync();

            var createdCustomerToReturn = _mapper.Map<CustomerWithoutListsDto>(finalCustomer);

            return CreatedAtRoute("GetCustomer",
                new
                {
                    id = createdCustomerToReturn.CustomerId
                },
                createdCustomerToReturn);
        }

        /// <summary>
        /// Update a customer by its ID
        /// </summary>
        /// <param name="id">Id of the customer</param>
        /// <param name="customer">Customer object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateCustomer(int id,
         CustomerForUpdateDto customer)
        {
            var customerEntity = await _customerRepository
                .GetCustomerAsync(id, false);

            if (customerEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(customer, customerEntity);

            await _customerRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a customer by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customerEntity = await _customerRepository
                .GetCustomerAsync(id, false);
            if (customerEntity == null)
            {
                return NotFound();
            }

            _customerRepository.DeleteCustomerAsync(customerEntity);
            await _customerRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
