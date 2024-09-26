using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a address entity with essential details
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Required field TenantId of the Address 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Address 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// AddressLine1 of the Address 
        /// </summary>
        public string? AddressLine1 { get; set; }
        /// <summary>
        /// AddressLine2 of the Address 
        /// </summary>
        public string? AddressLine2 { get; set; }
        /// <summary>
        /// AddressLine3 of the Address 
        /// </summary>
        public string? AddressLine3 { get; set; }
        /// <summary>
        /// CareOf of the Address 
        /// </summary>
        public string? CareOf { get; set; }
        /// <summary>
        /// POBox of the Address 
        /// </summary>
        public string? POBox { get; set; }
        /// <summary>
        /// Foreign key referencing the Country to which the Address belongs 
        /// </summary>
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Country
        /// </summary>
        [ForeignKey("CountryId")]
        public Country? CountryId_Country { get; set; }
        /// <summary>
        /// Foreign key referencing the City to which the Address belongs 
        /// </summary>
        public Guid? CityId { get; set; }

        /// <summary>
        /// Navigation property representing the associated City
        /// </summary>
        [ForeignKey("CityId")]
        public City? CityId_City { get; set; }
        /// <summary>
        /// MunicipalityId of the Address 
        /// </summary>
        public Guid? MunicipalityId { get; set; }
        /// <summary>
        /// Foreign key referencing the State to which the Address belongs 
        /// </summary>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Navigation property representing the associated State
        /// </summary>
        [ForeignKey("StateId")]
        public State? StateId_State { get; set; }
        /// <summary>
        /// PostalCode of the Address 
        /// </summary>
        public string? PostalCode { get; set; }
        /// <summary>
        /// AddressLine4 of the Address 
        /// </summary>
        public string? AddressLine4 { get; set; }
    }
}