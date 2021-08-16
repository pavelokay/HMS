using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Range(1, 1000)]
        public int FloorNumber { get; set; }
        [Range(1, 100000)]
        public int Number { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        public int RoomStatusId { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
