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
    /// The cityService responsible for managing city related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting city information.
    /// </remarks>
    public interface ICityService
    {
        /// <summary>Retrieves a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The city data</returns>
        Task<dynamic> GetById(Guid id, string fields);

        /// <summary>Retrieves a list of citys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of citys</returns>
        Task<List<City>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new city</summary>
        /// <param name="model">The city data to be added</param>
        /// <returns>The result of the operation</returns>
        Task<Guid> Create(City model);

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Update(Guid id, City updatedEntity);

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Patch(Guid id, JsonPatchDocument<City> updatedEntity);

        /// <summary>Deletes a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Delete(Guid id);
    }

    /// <summary>
    /// The cityService responsible for managing city related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting city information.
    /// </remarks>
    public class CityService : ICityService
    {
        private readonly Patient_testContext _dbContext;
        private readonly IFieldMapperService _mapper;

        /// <summary>
        /// Initializes a new instance of the City class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        /// <param name="mapper">mapper value to set.</param>
        public CityService(Patient_testContext dbContext, IFieldMapperService mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>Retrieves a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The city data</returns>
        public async Task<dynamic> GetById(Guid id, string fields)
        {
            var query = _dbContext.City.AsQueryable();
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

            string[] navigationProperties = ["CountryId_Country","StateId_State"];
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

        /// <summary>Retrieves a list of citys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of citys</returns>/// <exception cref="Exception"></exception>
        public async Task<List<City>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = await GetCity(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new city</summary>
        /// <param name="model">The city data to be added</param>
        /// <returns>The result of the operation</returns>
        public async Task<Guid> Create(City model)
        {
            model.Id = await CreateCity(model);
            return model.Id;
        }

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Update(Guid id, City updatedEntity)
        {
            await UpdateCity(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Patch(Guid id, JsonPatchDocument<City> updatedEntity)
        {
            await PatchCity(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(Guid id)
        {
            await DeleteCity(id);
            return true;
        }
        #region
        private async Task<List<City>> GetCity(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.City.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<City>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(City), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<City, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private async Task<Guid> CreateCity(City model)
        {
            _dbContext.City.Add(model);
            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        private async Task UpdateCity(Guid id, City updatedEntity)
        {
            _dbContext.City.Update(updatedEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> DeleteCity(Guid id)
        {
            var entityData = _dbContext.City.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.City.Remove(entityData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PatchCity(Guid id, JsonPatchDocument<City> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.City.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.City.Update(existingEntity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}