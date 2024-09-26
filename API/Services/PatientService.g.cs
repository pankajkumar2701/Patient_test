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
    /// The patientService responsible for managing patient related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patient information.
    /// </remarks>
    public interface IPatientService
    {
        /// <summary>Retrieves a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The patient data</returns>
        Task<dynamic> GetById(Guid id, string fields);

        /// <summary>Retrieves a list of patients based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patients</returns>
        Task<List<Patient>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patient</summary>
        /// <param name="model">The patient data to be added</param>
        /// <returns>The result of the operation</returns>
        Task<Guid> Create(Patient model);

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Update(Guid id, Patient updatedEntity);

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Patch(Guid id, JsonPatchDocument<Patient> updatedEntity);

        /// <summary>Deletes a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <returns>The result of the operation</returns>
        Task<bool> Delete(Guid id);
    }

    /// <summary>
    /// The patientService responsible for managing patient related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patient information.
    /// </remarks>
    public class PatientService : IPatientService
    {
        private readonly Patient_testContext _dbContext;
        private readonly IFieldMapperService _mapper;

        /// <summary>
        /// Initializes a new instance of the Patient class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        /// <param name="mapper">mapper value to set.</param>
        public PatientService(Patient_testContext dbContext, IFieldMapperService mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>Retrieves a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="fields">The fields is fetch data of selected fields</param>
        /// <returns>The patient data</returns>
        public async Task<dynamic> GetById(Guid id, string fields)
        {
            var query = _dbContext.Patient.AsQueryable();
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

            string[] navigationProperties = ["Gender_Gender","AgeUnit_AgeUnit","ReferredById_Contact","LocationId_Location","MembershipId_Membership","Title_Title","PatientAddressId_Address"];
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

        /// <summary>Retrieves a list of patients based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patients</returns>/// <exception cref="Exception"></exception>
        public async Task<List<Patient>> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = await GetPatient(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patient</summary>
        /// <param name="model">The patient data to be added</param>
        /// <returns>The result of the operation</returns>
        public async Task<Guid> Create(Patient model)
        {
            model.Id = await CreatePatient(model);
            return model.Id;
        }

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Update(Guid id, Patient updatedEntity)
        {
            await UpdatePatient(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Patch(Guid id, JsonPatchDocument<Patient> updatedEntity)
        {
            await PatchPatient(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(Guid id)
        {
            await DeletePatient(id);
            return true;
        }
        #region
        private async Task<List<Patient>> GetPatient(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Patient.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Patient>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Patient), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Patient, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private async Task<Guid> CreatePatient(Patient model)
        {
            _dbContext.Patient.Add(model);
            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        private async Task UpdatePatient(Guid id, Patient updatedEntity)
        {
            _dbContext.Patient.Update(updatedEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> DeletePatient(Guid id)
        {
            var entityData = _dbContext.Patient.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Patient.Remove(entityData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PatchPatient(Guid id, JsonPatchDocument<Patient> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Patient.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Patient.Update(existingEntity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}