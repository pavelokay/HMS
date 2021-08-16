using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace HMS.Domain.Models
{
    public class User : IdentityUser
    {

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
        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
    }
}
