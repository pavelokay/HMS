using HMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Services.Common
{
    public interface IReservationService
    {
        Task<ICollection<RoomType>> FindRoomTypesAsync(int hotelId, DateTime checkIn, DateTime checkOut, int? guestCount, int? roomCount);
        Task<Reservation> MakeReservationAsync(int roomTypeId, DateTime checkIn, DateTime checkOut, int guestCount, int roomCount);
    }
}
