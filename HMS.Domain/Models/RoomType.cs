using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Domain.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }

        [DataType(DataType.Text)]
        public decimal Size { get; set; }

        [StringLength(1000)]
        public string Decor { get; set; }

        [StringLength(1000)]
        public string Bathroom { get; set; }

        [StringLength(1000)]
        public string Bed { get; set; }

        [StringLength(1000)]
        public string Features { get; set; }

        [StringLength(1000)]
        public string View { get; set; }
        
        [DataType(DataType.Text)]
        public decimal? Rating { get; set; }

        [Range(1, 20)]
        public int MaxGuest { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<RoomTypeImage> RoomTypeImages { get; set; }
    }
}
