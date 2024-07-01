using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{

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

        [HttpGet]
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

        [HttpGet("{id}", Name = "GetCustomer")]
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

        [HttpPost]
        public async Task<ActionResult<CustomerWithoutListsDto>> CreateCustomer(CustomerForCreationDto customer)
        {
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePointOfInterest(int id)
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
