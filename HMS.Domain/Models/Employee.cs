using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class Employee : User
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        public string Position { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
