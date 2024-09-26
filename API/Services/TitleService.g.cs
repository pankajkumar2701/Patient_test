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
    /// The titleService responsible for managing title related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting title information.
    /// </remarks>
    public interface ITitleService
    {
        /// <summary>Retrieves a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The title data</returns>
        Task<dynamic> GetById(Guid id, string fields);

        /// <summary>Retrieves a list of titles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of titles</returns>
        Task<List<Title>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new title</summary>
        /// <param name="model">The title data to be added</param>
        /// <returns>The result of the operation</returns>
        Task<Guid> Create(Title model);

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Update(Guid id, Title updatedEntity);

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Patch(Guid id, JsonPatchDocument<Title> updatedEntity);

        /// <summary>Deletes a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Delete(Guid id);
    }

    /// <summary>
    /// The titleService responsible for managing title related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting title information.
    /// </remarks>
    public class TitleService : ITitleService
    {
        private readonly Patient_testContext _dbContext;
        private readonly IFieldMapperService _mapper;

        /// <summary>
        /// Initializes a new instance of the Title class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        /// <param name="mapper">mapper value to set.</param>
        public TitleService(Patient_testContext dbContext, IFieldMapperService mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>Retrieves a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The title data</returns>
        public async Task<dynamic> GetById(Guid id, string fields)
        {
            var query = _dbContext.Title.AsQueryable();
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

        /// <summary>Retrieves a list of titles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of titles</returns>/// <exception cref="Exception"></exception>
        public async Task<List<Title>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = await GetTitle(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new title</summary>
        /// <param name="model">The title data to be added</param>
        /// <returns>The result of the operation</returns>
        public async Task<Guid> Create(Title model)
        {
            model.Id = await CreateTitle(model);
            return model.Id;
        }

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Update(Guid id, Title updatedEntity)
        {
            await UpdateTitle(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Patch(Guid id, JsonPatchDocument<Title> updatedEntity)
        {
            await PatchTitle(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(Guid id)
        {
            await DeleteTitle(id);
            return true;
        }
        #region
        private async Task<List<Title>> GetTitle(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Title.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Title>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Title), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Title, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private async Task<Guid> CreateTitle(Title model)
        {
            _dbContext.Title.Add(model);
            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        private async Task UpdateTitle(Guid id, Title updatedEntity)
        {
            _dbContext.Title.Update(updatedEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> DeleteTitle(Guid id)
        {
            var entityData = _dbContext.Title.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Title.Remove(entityData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PatchTitle(Guid id, JsonPatchDocument<Title> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Title.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Title.Update(existingEntity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}