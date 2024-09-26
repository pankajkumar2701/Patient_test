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
    /// The contactService responsible for managing contact related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting contact information.
    /// </remarks>
    public interface IContactService
    {
        /// <summary>Retrieves a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The contact data</returns>
        Task<dynamic> GetById(Guid id, string fields);

        /// <summary>Retrieves a list of contacts based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of contacts</returns>
        Task<List<Contact>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new contact</summary>
        /// <param name="model">The contact data to be added</param>
        /// <returns>The result of the operation</returns>
        Task<Guid> Create(Contact model);

        /// <summary>Updates a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <param name="updatedEntity">The contact data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Update(Guid id, Contact updatedEntity);

        /// <summary>Updates a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <param name="updatedEntity">The contact data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Patch(Guid id, JsonPatchDocument<Contact> updatedEntity);

        /// <summary>Deletes a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Delete(Guid id);
    }

    /// <summary>
    /// The contactService responsible for managing contact related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting contact information.
    /// </remarks>
    public class ContactService : IContactService
    {
        private readonly Patient_testContext _dbContext;
        private readonly IFieldMapperService _mapper;

        /// <summary>
        /// Initializes a new instance of the Contact class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        /// <param name="mapper">mapper value to set.</param>
        public ContactService(Patient_testContext dbContext, IFieldMapperService mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>Retrieves a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The contact data</returns>
        public async Task<dynamic> GetById(Guid id, string fields)
        {
            var query = _dbContext.Contact.AsQueryable();
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

            string[] navigationProperties = ["TitleId_Title","StateId_State","CityId_City","CountryId_Country"];
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

        /// <summary>Retrieves a list of contacts based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of contacts</returns>/// <exception cref="Exception"></exception>
        public async Task<List<Contact>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = await GetContact(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new contact</summary>
        /// <param name="model">The contact data to be added</param>
        /// <returns>The result of the operation</returns>
        public async Task<Guid> Create(Contact model)
        {
            model.Id = await CreateContact(model);
            return model.Id;
        }

        /// <summary>Updates a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <param name="updatedEntity">The contact data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Update(Guid id, Contact updatedEntity)
        {
            await UpdateContact(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <param name="updatedEntity">The contact data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Patch(Guid id, JsonPatchDocument<Contact> updatedEntity)
        {
            await PatchContact(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific contact by its primary key</summary>
        /// <param name="id">The primary key of the contact</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(Guid id)
        {
            await DeleteContact(id);
            return true;
        }
        #region
        private async Task<List<Contact>> GetContact(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Contact.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Contact>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Contact), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Contact, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private async Task<Guid> CreateContact(Contact model)
        {
            _dbContext.Contact.Add(model);
            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        private async Task UpdateContact(Guid id, Contact updatedEntity)
        {
            _dbContext.Contact.Update(updatedEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> DeleteContact(Guid id)
        {
            var entityData = _dbContext.Contact.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Contact.Remove(entityData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PatchContact(Guid id, JsonPatchDocument<Contact> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Contact.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Contact.Update(existingEntity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}