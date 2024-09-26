using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a country entity with essential details
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Required field TenantId of the Country 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Country 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field CreatedBy of the Country 
        /// </summary>
        [Required]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Required field CreatedOn of the Country 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// ParentId of the Country 
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// UpdatedBy of the Country 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Country 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// DecimalPrecision of the Country 
        /// </summary>
        public int? DecimalPrecision { get; set; }
        /// <summary>
        /// LongDateFormat of the Country 
        /// </summary>
        public Guid? LongDateFormat { get; set; }
        /// <summary>
        /// numericDateFormat of the Country 
        /// </summary>
        public Guid? numericDateFormat { get; set; }
        /// <summary>
        /// Status of the Country 
        /// </summary>
        public bool? Status { get; set; }
        /// <summary>
        /// CalculationPrecision of the Country 
        /// </summary>
        public int? CalculationPrecision { get; set; }
        /// <summary>
        /// DisplayPrecision of the Country 
        /// </summary>
        public int? DisplayPrecision { get; set; }
        /// <summary>
        /// ThousandSeparator of the Country 
        /// </summary>
        public bool? ThousandSeparator { get; set; }
        /// <summary>
        /// Code of the Country 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Country 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Currency of the Country 
        /// </summary>
        public Guid? Currency { get; set; }
        /// <summary>
        /// Language of the Country 
        /// </summary>
        public Guid? Language { get; set; }
        /// <summary>
        /// Timezone of the Country 
        /// </summary>
        public Guid? Timezone { get; set; }
        /// <summary>
        /// IsoCode of the Country 
        /// </summary>
        public string? IsoCode { get; set; }
        /// <summary>
        /// Nationality of the Country 
        /// </summary>
        public string? Nationality { get; set; }
        /// <summary>
        /// CountryImage of the Country 
        /// </summary>
        public string? CountryImage { get; set; }
        /// <summary>
        /// Active of the Country 
        /// </summary>
        public Guid? Active { get; set; }
    }
}