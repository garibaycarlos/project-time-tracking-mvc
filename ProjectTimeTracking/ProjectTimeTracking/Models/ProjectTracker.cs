using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTimeTracking.Models
{
    public class ProjectTracker
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        
        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        
        [Required]
        [Display(Name = "Resource")]
        public int ResourceId { get; set; }
        
        [Required]
        [Display(Name = "Status")]
        public int ProjectStatusId { get; set; }
        
        [MaxLength(100)]
        public string Initiator { get; set; }
        
        [MaxLength(150)]
        public string Description { get; set; }
        
        public int? Hours { get; set; }
        
        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }
        
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        
        [Display(Name = "Completion Date")]
        public DateTime? CompletionDate { get; set; }
        
        [MaxLength(255)]
        public string CreatedBy { get; set; }
        
        public DateTime? DateCreated { get; set; }
        
        [MaxLength(255)]
        public string Modifiedby { get; set; }
        
        public DateTime? DateModified { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; }

        [ForeignKey("ProjectStatusId")]
        public virtual ProjectStatus ProjectStatus { get; set; }
    }
}