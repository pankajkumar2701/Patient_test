using Microsoft.AspNetCore.Mvc;
using Patient_test.Models;
using Patient_test.Services;
using Patient_test.Entities;
using Patient_test.Filter;
using Patient_test.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Task = System.Threading.Tasks.Task;
using Patient_test.Authorization;

namespace Patient_test.Controllers
{
    /// <summary>
    /// Controller responsible for managing ageunit related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting ageunit information.
    /// </remarks>
    [Route("api/ageunit")]
    [Authorize]
    public class AgeUnitController : BaseApiController
    {
        private readonly IAgeUnitService _ageUnitService;

        /// <summary>
        /// Initializes a new instance of the AgeUnitController class with the specified context.
        /// </summary>
        /// <param name="iageunitservice">The iageunitservice to be used by the controller.</param>
        public AgeUnitController(IAgeUnitService iageunitservice)
        {
            _ageUnitService = iageunitservice;
        }

        /// <summary>Adds a new ageunit</summary>
        /// <param name="model">The ageunit data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("AgeUnit", Entitlements.Create)]
        public async Task<IActionResult> Post([FromBody] AgeUnit model)
        {
            model.TenantId = TenantId;
            model.CreatedBy = UserId;
            model.CreatedOn = DateTime.UtcNow;
            var id = await _ageUnitService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of ageunits based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of ageunits</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [UserAuthorize("AgeUnit", Entitlements.Read)]
        public async Task<IActionResult> Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var result = await _ageUnitService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The ageunit data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("AgeUnit", Entitlements.Read)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, string fields = null)
        {
            var result = await _ageUnitService.GetById( id, fields);
            return Ok(result);
        }

        /// <summary>Deletes a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        [UserAuthorize("AgeUnit", Entitlements.Delete)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var status = await _ageUnitService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="updatedEntity">The ageunit data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [UserAuthorize("AgeUnit", Entitlements.Update)]
        public async Task<IActionResult> UpdateById(Guid id, [FromBody] AgeUnit updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            updatedEntity.TenantId = TenantId;
            updatedEntity.UpdatedBy = UserId;
            updatedEntity.UpdatedOn = DateTime.UtcNow;
            var status = await _ageUnitService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="updatedEntity">The ageunit data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [UserAuthorize("AgeUnit", Entitlements.Update)]
        public async Task<IActionResult> UpdateById(Guid id, [FromBody] JsonPatchDocument<AgeUnit> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = await _ageUnitService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}