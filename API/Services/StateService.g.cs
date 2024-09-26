using Patient_test.Models;
using Patient_test.Data;
using Patient_test.Filter;
using Patient_test.Entities;
using Patient_test.Logger;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using Task = System.Threading.Tasks.Task;

namespace Patient_test.Services
{
    /// <summary>
    /// The stateService responsible for managing state related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting state information.
    /// </remarks>
    public interface IStateService
    {
        /// <summary>Retrieves a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The state data</returns>
        Task<dynamic> GetById(Guid id, string fields);

        /// <summary>Retrieves a list of states based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of states</returns>
        Task<List<State>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new state</summary>
        /// <param name="model">The state data to be added</param>
        /// <returns>The result of the operation</returns>
        Task<Guid> Create(State model);

        /// <summary>Updates a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <param name="updatedEntity">The state data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Update(Guid id, State updatedEntity);

        /// <summary>Updates a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <param name="updatedEntity">The state data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Patch(Guid id, JsonPatchDocument<State> updatedEntity);

        /// <summary>Deletes a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Delete(Guid id);
    }

    /// <summary>
    /// The stateService responsible for managing state related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting state information.
    /// </remarks>
    public class StateService : IStateService
    {
        private readonly Patient_testContext _dbContext;
        private readonly IFieldMapperService _mapper;

        /// <summary>
        /// Initializes a new instance of the State class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        /// <param name="mapper">mapper value to set.</param>
        public StateService(Patient_testContext dbContext, IFieldMapperService mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>Retrieves a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The state data</returns>
        public async Task<dynamic> GetById(Guid id, string fields)
        {
            var query = _dbContext.State.AsQueryable();
            List<string> allfields = new List<string>();
            if (!string.IsNullOrEmpty(fields))
            {
                allfields.AddRange(fields.Split(","));
                fields = $"Id,{fields}";
            }
            else
            {
                fields = "Id";
            }

            string[] navigationProperties = ["CountryId_Country"];
            foreach (var navigationProperty in navigationProperties)
            {
                if (allfields.Any(field => field.StartsWith(navigationProperty + ".", StringComparison.OrdinalIgnoreCase)))
                {
                    query = query.Include(navigationProperty);
                }
            }

            query = query.Where(entity => entity.Id == id);
            return _mapper.MapToFields(await query.FirstOrDefaultAsync(),fields);
        }

        /// <summary>Retrieves a list of states based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of states</returns>/// <exception cref="Exception"></exception>
        public async Task<List<State>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = await GetState(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new state</summary>
        /// <param name="model">The state data to be added</param>
        /// <returns>The result of the operation</returns>
        public async Task<Guid> Create(State model)
        {
            model.Id = await CreateState(model);
            return model.Id;
        }

        /// <summary>Updates a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <param name="updatedEntity">The state data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Update(Guid id, State updatedEntity)
        {
            await UpdateState(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <param name="updatedEntity">The state data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Patch(Guid id, JsonPatchDocument<State> updatedEntity)
        {
            await PatchState(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific state by its primary key</summary>
        /// <param name="id">The primary key of the state</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(Guid id)
        {
            await DeleteState(id);
            return true;
        }
        #region
        private async Task<List<State>> GetState(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.State.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<State>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(State), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<State, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(lambda);
                }
                else if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(lambda);
                }
                else
                {
                    throw new ApplicationException("Invalid sort order. Use 'asc' or 'desc'");
                }
            }

            var paginatedResult = await result.Skip(skip).Take(pageSize).ToListAsync();
            return paginatedResult;
        }

        private async Task<Guid> CreateState(State model)
        {
            _dbContext.State.Add(model);
            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        private async Task UpdateState(Guid id, State updatedEntity)
        {
            _dbContext.State.Update(updatedEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> DeleteState(Guid id)
        {
            var entityData = _dbContext.State.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.State.Remove(entityData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PatchState(Guid id, JsonPatchDocument<State> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.State.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.State.Update(existingEntity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}