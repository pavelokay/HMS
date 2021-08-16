using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Domain.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public int HotelComplexId { get; set; }
        public HotelComplex HotelComplex { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(40)]
        public string Town { get; set; }
        [Required]
        [StringLength(40)]
        public string Country { get; set; }
        [Required]
        [StringLength(70)]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        [StringLength(100)]
        public string Site { get; set; }

        [Range(1, 5)]
        public int? Stars { get; set; }

        [Range(0, 1000)]
        public int FloorCount { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public ICollection<RoomType> RoomTypes { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; }

        public ICollection<Department> Departments { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}
