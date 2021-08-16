using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Description { get; set; }
    }
}
