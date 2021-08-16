using HMS.Domain.Models;
using System.Collections.Generic;


namespace HMS.Web.ViewModels.Statistics
{
    public class UserStatisticsViewModel
    {
        public IList<User> Users { get; set; }
        public IList<Role> AllRoles { get; set; }
        public IList<IList<string>> UserRoles { get; set; }
        public IList<Service> Services { get; set; }

        public int? ModeId { get; set; }
        public int? HotelId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public int? MinReservationRoomCount { get; set; }
        public int? TopPlaceCount { get; set; }
        public string DateRange { get; set; }
        public string FullPeriod { get; set; }
        public string DateRange2 { get; set; }
        public int? MinFloorNumber { get; set; }
        public int? MaxFloorNumber { get; set; }
        public int? MinGuest { get; set; }
        public int? MaxGuest { get; set; }
        public int? RoomStatusId { get; set; }
        public decimal? MinRate { get; set; }
        public decimal? MaxRate { get; set; }
    }
}
