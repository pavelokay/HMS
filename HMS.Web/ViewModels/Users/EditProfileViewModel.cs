using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Web.ViewModels.Users
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        
        [MinLength(6, ErrorMessage = "at least 6 characters")]
        [MaxLength(15, ErrorMessage = "maximum 15 characters")]
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string Country { get; set; }

        [Required]
        [StringLength(60)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string Town { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public IList<string> Roles { get; set; }
    }
}
