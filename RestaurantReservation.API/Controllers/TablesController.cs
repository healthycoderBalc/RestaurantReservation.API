using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Table;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/tables")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableWithoutListsDto>>> GetTables()
        {
            var tableEntities = await _tableRepository.GetTablesAsync();
            return Ok(_mapper.Map<IEnumerable<TableWithoutListsDto>>(tableEntities));
        }

        [HttpGet("{id}", Name = "GetTable")]
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

        [HttpPost]
        public async Task<ActionResult<TableDto>> CreateTable(
    TableForCreationDto table)
        {
            var restaurantId = table.RestaurantId;
            if (!await _tableRepository.RestaurantExistsAsync(restaurantId))
            {
                return NotFound();
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
