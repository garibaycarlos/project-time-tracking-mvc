using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTimeTracking.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [MaxLength(255)]
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        [MaxLength(255)]
        public string Modifiedby { get; set; }
        public DateTime? DateModified { get; set; }
    }
}