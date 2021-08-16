using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class HotelComplex
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
        
        public ICollection<HotelComplexImage> HotelComplexImages { get; set; }
    }
}
