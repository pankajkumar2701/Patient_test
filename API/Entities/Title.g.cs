using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Patient_test.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a title entity with essential details
    /// </summary>
    public class Title
    {
        /// <summary>
        /// Required field TenantId of the Title 
        /// </summary>
        [Required]
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Title 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field CreatedBy of the Title 
        /// </summary>
        [Required]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Required field CreatedOn of the Title 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// ParentId of the Title 
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// Code of the Title 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Title 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Active of the Title 
        /// </summary>
        public Guid? Active { get; set; }
        /// <summary>
        /// UpdatedBy of the Title 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Title 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}