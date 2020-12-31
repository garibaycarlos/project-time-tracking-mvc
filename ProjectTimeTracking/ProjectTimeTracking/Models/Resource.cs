using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTimeTracking.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName{ get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The Email is not a valid e-mail address.")]
        [MaxLength(150)]
        public string Email{ get; set; }
        [Required]
        public bool IsActive { get; set; }
        [MaxLength(255)]
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        [MaxLength(255)]
        public string Modifiedby { get; set; }
        public DateTime? DateModified { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}