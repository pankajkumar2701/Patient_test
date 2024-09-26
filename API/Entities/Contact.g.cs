using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a contact entity with essential details
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Required field FirstName of the Contact 
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Required field CreatedBy of the Contact 
        /// </summary>
        [Required]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Required field CreatedOn of the Contact 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Primary key for the Contact 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field TenantId of the Contact 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }
        /// <summary>
        /// Foreign key referencing the Title to which the Contact belongs 
        /// </summary>
        public Guid? TitleId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Title
        /// </summary>
        [ForeignKey("TitleId")]
        public Title? TitleId_Title { get; set; }
        /// <summary>
        /// UpdatedBy of the Contact 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Contact 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Code of the Contact 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Contact 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// RepeatVisitSpan of the Contact 
        /// </summary>
        public int? RepeatVisitSpan { get; set; }
        /// <summary>
        /// BusinessRegistrationNumber of the Contact 
        /// </summary>
        public string? BusinessRegistrationNumber { get; set; }
        /// <summary>
        /// JobTitle of the Contact 
        /// </summary>
        public string? JobTitle { get; set; }
        /// <summary>
        /// CountryCode of the Contact 
        /// </summary>
        public int? CountryCode { get; set; }
        /// <summary>
        /// Mobile of the Contact 
        /// </summary>
        public string? Mobile { get; set; }
        /// <summary>
        /// Email of the Contact 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Notes of the Contact 
        /// </summary>
        public string? Notes { get; set; }
        /// <summary>
        /// AddressLine1 of the Contact 
        /// </summary>
        public string? AddressLine1 { get; set; }
        /// <summary>
        /// AddressLine2 of the Contact 
        /// </summary>
        public string? AddressLine2 { get; set; }
        /// <summary>
        /// Foreign key referencing the State to which the Contact belongs 
        /// </summary>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Navigation property representing the associated State
        /// </summary>
        [ForeignKey("StateId")]
        public State? StateId_State { get; set; }
        /// <summary>
        /// Foreign key referencing the City to which the Contact belongs 
        /// </summary>
        public Guid? CityId { get; set; }

        /// <summary>
        /// Navigation property representing the associated City
        /// </summary>
        [ForeignKey("CityId")]
        public City? CityId_City { get; set; }
        /// <summary>
        /// Foreign key referencing the Country to which the Contact belongs 
        /// </summary>
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Country
        /// </summary>
        [ForeignKey("CountryId")]
        public Country? CountryId_Country { get; set; }
        /// <summary>
        /// PostalCode of the Contact 
        /// </summary>
        public string? PostalCode { get; set; }
    }
}