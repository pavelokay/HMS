using HMS.Domain.Models;
using System;
using System.Collections.Generic;

namespace HMS.Web.ViewModels.ReservationController
{
    public class ChooseRoomViewModel
    {
        public ICollection<RoomType> RoomTypes { get; set; }
        public Hotel Hotel { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? RoomCount { get; set; }
        public int? GuestCount { get; set; }
    }
}
