using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class Department
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
    }
}
