using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a membership entity with essential details
    /// </summary>
    public class Membership
    {
        /// <summary>
        /// Required field TenantId of the Membership 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Membership 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field SerialNumber of the Membership 
        /// </summary>
        [Required]
        public int SerialNumber { get; set; }

        /// <summary>
        /// Required field Name of the Membership 
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Required field Code of the Membership 
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Required field CreatedBy of the Membership 
        /// </summary>
        [Required]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Required field CreatedOn of the Membership 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Membership 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Membership 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Notes of the Membership 
        /// </summary>
        public string? Notes { get; set; }
    }
}