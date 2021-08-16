using HMS.Domain.Models;
using HMS.Domain.UnitOfWork;
using HMS.Web.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Services
{
    public class ReservationService : IReservationService
    {
        private IUnitOfWork unitOfWork;

        public ReservationService(
           IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICollection<RoomType>> FindRoomTypesAsync(int hotelId, DateTime checkIn, DateTime checkOut, int? guestCount, int? roomCount)
        {
            var roomTypesDB = await unitOfWork.RoomTypes.GetAsync(
                rt => rt.HotelId == hotelId,
                include: rt => rt.Include(rooms => rooms.Rooms).Include(img => img.RoomTypeImages),
                disableTracking: true);

            var roomTypes = roomTypesDB.ToList();
            roomTypes.RemoveAll(rt => rt.Rooms.Count() < roomCount);
            roomTypes.RemoveAll(rt => rt.MaxGuest < guestCount);

            var hotelRoomTypes = roomTypes.Select(rt => rt.Id);

            var reservationsDB = await unitOfWork.Reservations.GetAsync(
                reserv => (reserv.CheckIn >= checkIn && reserv.CheckIn <= checkOut) || (reserv.CheckIn <= checkIn && reserv.CheckOut >= checkIn),
                include: reserv => reserv.Include(room => room.Rooms),
                disableTracking: true);

            var reservations = reservationsDB.ToList();
            reservations = reservations.Where(reserv => reserv.Rooms.Any(room => hotelRoomTypes.Contains(room.RoomTypeId))).ToList();

            for (int i = 0; i < reservations.Count(); i++)
            {
                var rooms = reservations[i].Rooms.ToList();
                for (int j = 0; j < rooms.Count(); j++)
                {
                    var typeId = rooms[j].RoomTypeId;
                    if (hotelRoomTypes.Contains(typeId))
                    {
                        roomTypes.FirstOrDefault(rt => rt.Id == typeId).Rooms.Remove(rooms[j]);
                    }
                }
            }

            roomTypes = roomTypes.Where(rt => rt.Rooms.Count() > 0).ToList();

            return roomTypes;
        }

        public async Task<Reservation> MakeReservationAsync(int roomTypeId, DateTime checkIn, DateTime checkOut, int guestCount, int roomCount)
        {
            var roomsDB = await unitOfWork.Rooms.GetAsync(r => r.RoomTypeId == roomTypeId);
            var rooms = roomsDB.ToList();

            var reservationsDB = await unitOfWork.Reservations.GetAsync(
                reserv => (reserv.CheckIn >= checkIn && reserv.CheckIn <= checkOut) || (reserv.CheckIn <= checkIn && reserv.CheckOut >= checkIn),
                include: reserv => reserv.Include(room => room.Rooms),
                disableTracking: true);

            var reservations = reservationsDB.ToList();
            reservations = reservations.Where(reserv => reserv.Rooms.Any(room => room.RoomTypeId == roomTypeId)).ToList();

            for (int i = 0; i < reservations.Count(); i++)
            {
                var reservationRooms = reservations[i].Rooms.ToList();
                for (int j = 0; j < reservationRooms.Count(); j++)
                {
                    if (reservationRooms[j].RoomTypeId == roomTypeId)
                    {
                        rooms.Remove(reservationRooms[j]);
                    }
                }
            }

            var reservation = new Reservation()
            {
                CheckIn = checkIn,
                CheckOut = checkOut,
                GuestCount = guestCount,
                RoomCount = roomCount,
                ReservationStatusId = 1,
                ReservationTypeId = 1,
                CreatedAt = DateTime.Now.Date,
                Rooms = new List<Room>()
            };

            for (int i = 0; i < roomCount; i++)
            {
                reservation.Rooms.Add(rooms[i]);
                if (reservation.CheckIn == DateTime.Today.Date)
                {
                    rooms[i].RoomStatusId = 2;
                    await unitOfWork.Rooms.UpdateAsync(rooms[i]);
                }
            }

            await unitOfWork.Reservations.AddAsync(reservation);
            await unitOfWork.SaveAsync();

            return reservation;
        }
    }
}
