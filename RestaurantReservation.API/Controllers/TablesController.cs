using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Contracts.Responses;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.Models.Table;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/tables")]
    [Authorize]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;
        public TablesController(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all tables
        /// </summary>
        /// <response code="200">Returns the tables</response>
        /// <returns>Tables</returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TableWithoutListsDto>>> GetTables()
        {
            var tableEntities = await _tableRepository.GetTablesAsync();
            return Ok(_mapper.Map<IEnumerable<TableWithoutListsDto>>(tableEntities));
        }

        /// <summary>
        /// Get a table by id
        /// </summary>
        /// <param name="id">table id</param>
        /// <param name="includeLists">Whether or not to include associated lists</param>
        /// <response code="200">Returns the requested table</response>
        /// <returns>A table</returns>
        [HttpGet("{id}", Name = "GetTable")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TableWithoutListsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTable(int id, bool includeLists = false)
        {
            var table = await _tableRepository.GetTableAsync(id, includeLists);
            if (table == null)
            {
                return NotFound();
            }
            if (includeLists)
            {
                return Ok(_mapper.Map<TableDto>(table));
            }
            return Ok(_mapper.Map<TableWithoutListsDto>(table));
        }

        /// <summary>
        /// Create a table
        /// </summary>
        /// <param name="table">Table data for creation</param>
        /// <response code="201">Returns the created table</response>
        /// <returns>The created table</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TableDto>> CreateTable(
    [FromBody] TableForCreationDto table)
        {
            var restaurantId = table.RestaurantId;
            if (!await _tableRepository.RestaurantExistsAsync(restaurantId))
            {
                return NotFound();
            }

            if (table == null)
            {
                return BadRequest();
            }

            var finalTable = _mapper.Map<Table>(table);

            await _tableRepository.CreateTableAsync(
                restaurantId, finalTable);

            await _tableRepository.SaveChangesAsync();

            var createdTableToReturn =
                _mapper.Map<TableWithoutListsDto>(finalTable);

            return CreatedAtRoute("GetTable",
                 new
                 {
                     id = createdTableToReturn.TableId
                 },
                 createdTableToReturn);
        }

        /// <summary>
        /// Update a table by id
        /// </summary>
        /// <param name="id">table id</param>
        /// <param name="table">Table object in json format</param>
        /// <response code="204">No content (update was successfull)</response>
        /// <returns>No content (update was successfull)</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateTable(int id,
         TableForUpdateDto table)
        {
            var restaurantId = table.RestaurantId;
            if (!await _tableRepository.RestaurantExistsAsync(restaurantId))
            {
                return NotFound();
            }

            var tableEntity = await _tableRepository
                .GetTableAsync(id, false);
            if (tableEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(table, tableEntity);

            await _tableRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a table by id
        /// </summary>
        /// <param name="id">table id</param>
        /// <response code="204">No content (Delete was successfull)</response>
        /// <returns>No content (Delete was successfull)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NotOkResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTable(int id)
        {
            var tableEntity = await _tableRepository
                .GetTableAsync(id, false);
            if (tableEntity == null)
            {
                return NotFound();
            }

            _tableRepository.DeleteTableAsync(tableEntity);
            await _tableRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
