using HMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.ViewModels.Statistics
{
    public class RoomStatisticsViewModel
    {
        public IList<Room> Rooms { get; set; }
        public int? ModeId { get; set; }
        public int? HotelId { get; set; }
        public string RoleName { get; set; }
        public string OutDay { get; set; }
        public int? MinFloorNumber { get; set; }
        public int? MaxFloorNumber { get; set; }
        public int? MinGuest { get; set; }
        public int? MaxGuest { get; set; }
        public int? RoomStatusId { get; set; }
        public decimal? MinRate { get; set; }
        public decimal? MaxRate { get; set; }
    }
}
