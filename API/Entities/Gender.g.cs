using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a gender entity with essential details
    /// </summary>
    public class Gender
    {
        /// <summary>
        /// Required field TenantId of the Gender 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Gender 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field CreatedBy of the Gender 
        /// </summary>
        [Required]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Required field CreatedOn of the Gender 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// ParentId of the Gender 
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// UpdatedBy of the Gender 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Gender 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Code of the Gender 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Gender 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Active of the Gender 
        /// </summary>
        public Guid? Active { get; set; }
    }
}