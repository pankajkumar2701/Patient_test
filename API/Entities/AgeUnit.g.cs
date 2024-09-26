using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a ageunit entity with essential details
    /// </summary>
    public class AgeUnit
    {
        /// <summary>
        /// Required field TenantId of the AgeUnit 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the AgeUnit 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field CreatedBy of the AgeUnit 
        /// </summary>
        [Required]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Required field CreatedOn of the AgeUnit 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// ParentId of the AgeUnit 
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// UpdatedBy of the AgeUnit 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the AgeUnit 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Code of the AgeUnit 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the AgeUnit 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Active of the AgeUnit 
        /// </summary>
        public Guid? Active { get; set; }
    }
}