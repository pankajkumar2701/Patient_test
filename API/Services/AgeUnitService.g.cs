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
    /// The ageunitService responsible for managing ageunit related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting ageunit information.
    /// </remarks>
    public interface IAgeUnitService
    {
        /// <summary>Retrieves a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The ageunit data</returns>
        Task<dynamic> GetById(Guid id, string fields);

        /// <summary>Retrieves a list of ageunits based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of ageunits</returns>
        Task<List<AgeUnit>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new ageunit</summary>
        /// <param name="model">The ageunit data to be added</param>
        /// <returns>The result of the operation</returns>
        Task<Guid> Create(AgeUnit model);

        /// <summary>Updates a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="updatedEntity">The ageunit data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Update(Guid id, AgeUnit updatedEntity);

        /// <summary>Updates a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="updatedEntity">The ageunit data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Patch(Guid id, JsonPatchDocument<AgeUnit> updatedEntity);

        /// <summary>Deletes a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Delete(Guid id);
    }

    /// <summary>
    /// The ageunitService responsible for managing ageunit related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting ageunit information.
    /// </remarks>
    public class AgeUnitService : IAgeUnitService
    {
        private readonly Patient_testContext _dbContext;
        private readonly IFieldMapperService _mapper;

        /// <summary>
        /// Initializes a new instance of the AgeUnit class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        /// <param name="mapper">mapper value to set.</param>
        public AgeUnitService(Patient_testContext dbContext, IFieldMapperService mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>Retrieves a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The ageunit data</returns>
        public async Task<dynamic> GetById(Guid id, string fields)
        {
            var query = _dbContext.AgeUnit.AsQueryable();
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

            string[] navigationProperties = [];
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

        /// <summary>Retrieves a list of ageunits based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of ageunits</returns>/// <exception cref="Exception"></exception>
        public async Task<List<AgeUnit>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = await GetAgeUnit(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new ageunit</summary>
        /// <param name="model">The ageunit data to be added</param>
        /// <returns>The result of the operation</returns>
        public async Task<Guid> Create(AgeUnit model)
        {
            model.Id = await CreateAgeUnit(model);
            return model.Id;
        }

        /// <summary>Updates a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="updatedEntity">The ageunit data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Update(Guid id, AgeUnit updatedEntity)
        {
            await UpdateAgeUnit(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <param name="updatedEntity">The ageunit data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Patch(Guid id, JsonPatchDocument<AgeUnit> updatedEntity)
        {
            await PatchAgeUnit(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific ageunit by its primary key</summary>
        /// <param name="id">The primary key of the ageunit</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(Guid id)
        {
            await DeleteAgeUnit(id);
            return true;
        }
        #region
        private async Task<List<AgeUnit>> GetAgeUnit(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.AgeUnit.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<AgeUnit>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(AgeUnit), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<AgeUnit, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private async Task<Guid> CreateAgeUnit(AgeUnit model)
        {
            _dbContext.AgeUnit.Add(model);
            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        private async Task UpdateAgeUnit(Guid id, AgeUnit updatedEntity)
        {
            _dbContext.AgeUnit.Update(updatedEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> DeleteAgeUnit(Guid id)
        {
            var entityData = _dbContext.AgeUnit.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.AgeUnit.Remove(entityData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PatchAgeUnit(Guid id, JsonPatchDocument<AgeUnit> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.AgeUnit.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.AgeUnit.Update(existingEntity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}