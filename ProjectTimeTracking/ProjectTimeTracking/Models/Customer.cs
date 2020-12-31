using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTimeTracking.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Customer")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Row Id")]
        public int RowId { get; set; }

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