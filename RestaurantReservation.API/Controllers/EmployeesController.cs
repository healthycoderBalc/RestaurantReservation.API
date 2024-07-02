using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/employees")]
    [Authorize]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeWithoutListsDto>>> GetEmployees([FromQuery] bool withRestaurantDetails = false)
        {
            if (withRestaurantDetails)
            {
                var employeeEntitiesWithRestaurantDetails = await _employeeRepository.GetEmployeesWithRestaurantDetailsFromViewAsync();
                return Ok(_mapper.Map<IEnumerable<EmployeeWithRestaurantDetailsDto>>(employeeEntitiesWithRestaurantDetails));
            }
            var employeeEntities = await _employeeRepository.GetEmployeesAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeWithoutListsDto>>(employeeEntities));

        }

        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<ActionResult> GetEmployee(int id, bool includeLists = false)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id, includeLists);
            if (employee == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<EmployeeDto>(employee));
            }
            return Ok(_mapper.Map<EmployeeWithoutListsDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(
             [FromBody] EmployeeForCreationDto employee)
        {
            var restaurantId = employee.RestaurantId;
            if (!await _employeeRepository.RestaurantExistsAsync(restaurantId))
            {
                return NotFound();
            }
            if (employee == null)
            {
                return BadRequest("Employee cannot be null");
            }
            var finalEmployee = _mapper.Map<Employee>(employee);

            await _employeeRepository.CreateEmployeeAsync(
                restaurantId, finalEmployee);

            await _employeeRepository.SaveChangesAsync();

            var createdEmployeeToReturn =
                _mapper.Map<EmployeeWithoutListsDto>(finalEmployee);

            return CreatedAtRoute("GetEmployee",
                 new
                 {
                     id = createdEmployeeToReturn.EmployeeId
                 },
                 createdEmployeeToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id,
         EmployeeForUpdateDto employee)
        {
            var restaurantId = employee.RestaurantId;
            if (!await _employeeRepository.RestaurantExistsAsync(restaurantId))
            {

                return NotFound();
            }

            var employeeEntity = await _employeeRepository
                .GetEmployeeAsync(id, false);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(employee, employeeEntity);

            await _employeeRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employeeEntity = await _employeeRepository
                .GetEmployeeAsync(id, false);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            _employeeRepository.DeleteEmployeeAsync(employeeEntity);
            await _employeeRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetManagers()
        {
            var managers = await _employeeRepository.ListManagersAsync();

            return Ok(_mapper.Map<IEnumerable<EmployeeWithoutListsDto>>(managers));
        }

        [HttpGet("{employeeId}/average-order-amount")]
        public async Task<ActionResult<decimal>> GetAverageOrderAmount(int employeeId)
        {
            var calculatedAverage = await _employeeRepository.CalculateAverageOrderAmountAsync(employeeId);
            return Ok(calculatedAverage);
        }
    }
}
