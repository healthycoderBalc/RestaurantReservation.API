using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    /// <summary>
    /// Employees endpoints
    /// </summary>
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

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <param name="withRestaurantDetails">Whether or not show the restaurant details for an employee</param>
        /// <response code="200">Returns the employees</response>
        /// <returns>Employees</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Get an employee by id
        /// </summary>
        /// <param name="id">Id of employee to get</param>
        /// <param name="includeLists">Whether or not to include the associated lists in the response</param>
        /// <response code="200">Returns the requested employee</response>
        /// <returns>An employee</returns>
        [HttpGet("{id}", Name = "GetEmployee")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EmployeeWithoutListsDto), StatusCodes.Status200OK)]
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

        /// <summary>
        ///  Create an employee
        /// </summary>
        /// <param name="employee">Employee data for creation</param>
        /// <response code="201">Returns the created employee</response>
        /// <returns>The created employee</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Update an employee by its ID
        /// </summary>
        /// <param name="id">Id of the employee</param>
        /// <param name="employee">Employee object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        ///  Delete an employee by id
        /// </summary>
        /// <param name="id">id of the employee for deletion</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Get all managers
        /// </summary>
        /// <response code="200">Employees who are managers</response>
        /// <returns>Employees</returns>
        [HttpGet("managers")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetManagers()
        {
            var managers = await _employeeRepository.ListManagersAsync();

            return Ok(_mapper.Map<IEnumerable<EmployeeWithoutListsDto>>(managers));
        }

        /// <summary>
        ///  Get average order amount by employee id
        /// </summary>
        /// <param name="employeeId">id of the employee</param>
        /// <response code="200">Average</response>
        /// <returns>Average</returns>
        [HttpGet("{employeeId}/average-order-amount")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetAverageOrderAmount(int employeeId)
        {
            var calculatedAverage = await _employeeRepository.CalculateAverageOrderAmountAsync(employeeId);
            return Ok(calculatedAverage);
        }
    }
}
