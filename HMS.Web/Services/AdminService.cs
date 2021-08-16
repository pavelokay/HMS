using HMS.Domain.Models;
using HMS.Domain.UnitOfWork;
using HMS.Web.Services.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Services
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork unitOfWork;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;

        public AdminService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<List<Room>> GetRoomStatisticsAsync(int? modeId, int? hotelId, string roleName, string outDay, int? minFloorNumber, int? maxFloorNumber, int? minGuest, int? maxGuest, int? roomStatusId, decimal? minRate, decimal? maxRate)
        {
            DateTime? checkOut = null;
            if (outDay != null)
            {
                checkOut = Convert.ToDateTime(outDay);
            }

            var roomTypesDB = await unitOfWork.RoomTypes.GetAsync(
                include: rt => rt
                .Include(rt => rt.Rooms)
                .Include(rt => rt.Hotel),
                disableTracking: true);
            var roomTypes = roomTypesDB.ToList();


            if (hotelId != null)
            {
                roomTypes = roomTypes.Where(rt => rt.HotelId == hotelId).ToList();
            }
            List<Room> rooms = new List<Room>();
            switch (modeId)
            {
                case 0:
                    foreach (var roomType in roomTypes)
                    {
                        rooms.AddRange(roomType.Rooms);
                    }
                    break;

                case 1:
                    foreach (var roomType in roomTypes)
                    {
                        foreach (var room in roomType.Rooms)
                        {
                            if (((minFloorNumber == null) || (room.FloorNumber >= minFloorNumber)) &&
                                ((maxFloorNumber == null) || (room.FloorNumber <= maxFloorNumber)) &&
                                ((minGuest == null) || (roomType.MaxGuest >= minGuest)) &&
                                ((maxGuest == null) || (roomType.MaxGuest <= maxGuest)) &&
                                ((minRate == null) || (roomType.Rate >= minRate)) &&
                                ((maxRate == null) || (roomType.Rate <= maxRate)) &&
                                ((roomStatusId == null) || (room.RoomStatusId == roomStatusId)))
                            {
                                rooms.Add(room);
                            }
                        }
                    }
                    break;

                case 2:
                    var reservationsDB = await unitOfWork.Reservations.GetAsync(
                        reserv => (reserv.CheckOut <= checkOut && reserv.CheckOut >= DateTime.Today.Date && reserv.CheckIn <= DateTime.Today.Date),
                        include: reserv => reserv
                        .Include(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel),
                        disableTracking: true);
                    var reservations = reservationsDB.ToList();


                    if (hotelId != null)
                    {
                        var hotelRoomTypes = roomTypes.Select(rt => rt.Id);
                        reservations = reservations.Where(reserv => reserv.Rooms.Any(room => hotelRoomTypes.Contains(room.RoomTypeId))).ToList();
                    }

                    for (int i = 0; i < reservations.Count(); i++)
                    {
                        var reservRooms = reservations[i].Rooms.ToList();

                        for (int j = 0; j < reservRooms.Count(); j++)
                        {
                            if (reservRooms[j].RoomStatusId == 2 && (hotelId == null || reservRooms[j].RoomType.HotelId == hotelId))
                            {
                                rooms.Add(reservRooms[j]);
                            }
                        }
                    }
                    break;

                case 3:
                    var allTransactionsDB = await unitOfWork.Transactions.GetAsync(
                        include: trans => trans
                        .Include(trans => trans.Client)
                        .Include(trans => trans.Reservation).ThenInclude(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel)
                        .Include(trans => trans.Services),
                        disableTracking: true);

                    foreach (var transaction in allTransactionsDB.Where(trans => trans.Services.Count() == 0))
                    {
                        var role = await userManager.GetRolesAsync(transaction.Client);
                        if (role.Contains(roleName))
                        {
                            rooms.AddRange(transaction.Reservation.Rooms);
                        }
                    }
                    break;
                default:
                    break;
            }
            return rooms;
        }

        public async Task<List<User>> GetUserStatisticsAsync(int? modeId, int? hotelId, string userId, string roleName, int? minReservationRoomCount, string dateRange, string fullPeriod, string dateRange2, int? minFloorNumber, int? maxFloorNumber, int? minGuest, int? maxGuest, int? roomStatusId, decimal? minRate, decimal? maxRate, string dateRangeReport, string dateRangeReport2, string reportFlag, int? topPlaceCount)
        {
            var checkIn = DateTime.Now;
            var checkOut = DateTime.Now;
            if (!String.IsNullOrEmpty(dateRange) && reportFlag != "1")
            {
                checkIn = Convert.ToDateTime(dateRange.Substring(0, 10));
                checkOut = Convert.ToDateTime(dateRange.Substring(13, 10));
            }
            if (!String.IsNullOrEmpty(dateRangeReport) && reportFlag == "1")
            {
                checkIn = Convert.ToDateTime(dateRangeReport.Substring(0, 10));
                checkOut = Convert.ToDateTime(dateRangeReport.Substring(13, 10));
            }
            List<User> users = new List<User>();
            var allClientsDB = await unitOfWork.Clients.GetAsync(
                include: u => u
                .Include(u => u.Transactions)
                .Include(u => u.Gender),
                disableTracking: true);
            var allUsers = allClientsDB.ToList();

            switch (modeId)
            {
                case 10:
                    foreach (var user in allUsers)
                    {
                        users.Add(user);
                    }
                    break;
                case 0:
                    if (roleName == null)
                    {
                        foreach (var user in allUsers)
                        {
                            var role = await userManager.GetRolesAsync(user);
                            if (role.Contains("client") || role.Contains("organization"))
                            {
                                users.Add(user);
                            }
                        }
                    }
                    else
                    {
                        foreach (var user in allUsers)
                        {
                            var role = await userManager.GetRolesAsync(user);
                            if (role.Contains(roleName))
                            {
                                users.Add(user);
                            }
                        }
                    }
                    break;
                case 1:

                    foreach (var user in allUsers)
                    {
                        var role = await userManager.GetRolesAsync(user);
                        if (role.Contains(roleName))
                        {
                            var userTransactionsBD = await unitOfWork.Transactions.GetAsync(
                                trans => trans.ClientId == userId,
                                include: trans => trans.Include(trans => trans.Reservation),
                                disableTracking: true);

                            foreach (var transaction in userTransactionsBD)
                            {
                                if (fullPeriod == "1")
                                {
                                    checkIn = DateTime.MinValue;
                                    checkOut = DateTime.MaxValue;
                                }
                                if (transaction.Reservation.CheckIn >= checkIn && transaction.Reservation.CheckOut <= checkOut && transaction.Reservation.RoomCount >= minReservationRoomCount)
                                {
                                    users.Add(user);
                                }
                            }
                        }
                    }
                    break;

                case 2:
                    foreach (var user in allUsers)
                    {
                        var role = await userManager.GetRolesAsync(user);
                        if (role.Contains(roleName))
                        {
                            bool quit = false;
                            var userTransactionsBD = await unitOfWork.Transactions.GetAsync(
                                trans => trans.ClientId == userId,
                                include: trans => trans
                                .Include(trans => trans.Reservation).ThenInclude(reserv => reserv.Rooms).ThenInclude(room => room.RoomType),
                                disableTracking: true);

                            foreach (var transaction in userTransactionsBD)
                            {
                                if (quit == false)
                                {
                                    if (fullPeriod == "1")
                                    {
                                        checkIn = DateTime.MinValue;
                                        checkOut = DateTime.MaxValue;
                                    }
                                    if (transaction.Reservation.CheckIn >= checkIn && transaction.Reservation.CheckOut <= checkOut)
                                    {
                                        foreach (var room in transaction.Reservation.Rooms)
                                        {
                                            var roomType = room.RoomType;
                                            if (((minFloorNumber == null) || (room.FloorNumber >= minFloorNumber)) &&
                                                ((maxFloorNumber == null) || (room.FloorNumber <= maxFloorNumber)) &&
                                                ((minGuest == null) || (roomType.MaxGuest >= minGuest)) &&
                                                ((maxGuest == null) || (roomType.MaxGuest <= maxGuest)) &&
                                                ((minRate == null) || (roomType.Rate >= minRate)) &&
                                                ((maxRate == null) || (roomType.Rate <= maxRate)))
                                            {
                                                users.Add(user);
                                                quit = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                case 3:
                    foreach (var user in allUsers)
                    {
                        var role = await userManager.GetRolesAsync(user);
                        if (role.Contains(roleName))
                        {
                            var userTransactionsBD = await unitOfWork.Transactions.GetAsync(
                                trans => trans.ClientId == userId,
                                include: trans => trans.Include(trans => trans.Reservation),
                                disableTracking: true);

                            foreach (var transaction in userTransactionsBD)
                            {
                                if (transaction.Reservation.CheckIn >= checkIn && transaction.Reservation.CheckOut <= checkOut)
                                {
                                    users.Add(user);
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case 4:
                    var userReservationCount = new List<int>();
                    int iteratorTop = 0;
                    topPlaceCount = topPlaceCount ?? 10;
                    for (int i = 0; i < allUsers.Count(); i++)
                    {
                        if (allUsers[i].Transactions.Any())
                        {
                            var userTransactions = await unitOfWork.Transactions.GetAsync(
                                trans => trans.ClientId == allUsers[i].Id,
                                include: trans => trans.Include(trans => trans.Services),
                                disableTracking: true);
                            var reservationTransactions = userTransactions.Where(t => t.Services.Count() == 0);

                            if (iteratorTop < topPlaceCount)
                            {
                                users.Add(allUsers[i]);
                                userReservationCount.Add(reservationTransactions.Count());
                                iteratorTop++;
                            }
                            else
                            {
                                if (userReservationCount.Min() < reservationTransactions.Count())
                                {
                                    var changePlace = userReservationCount.IndexOf(userReservationCount.Min());
                                    users[changePlace] = allUsers[i];
                                    userReservationCount[changePlace] = reservationTransactions.Count();
                                }
                            }
                        }
                    }
                    break;

                case 5:
                    var createdTimeBegin = DateTime.Today;
                    var createdTimeEnd = DateTime.Today;
                    if (!String.IsNullOrEmpty(dateRange2) && reportFlag != "1")
                    {
                        createdTimeBegin = Convert.ToDateTime(dateRange2.Substring(0, 10));
                        createdTimeEnd = Convert.ToDateTime(dateRange2.Substring(13, 10));
                    }
                    if (!String.IsNullOrEmpty(dateRangeReport2) && reportFlag == "1")
                    {
                        createdTimeBegin = Convert.ToDateTime(dateRangeReport2.Substring(0, 10));
                        createdTimeEnd = Convert.ToDateTime(dateRangeReport2.Substring(13, 10));
                    }
                    foreach (var client in allUsers)
                    {
                        if ((client.CreatedAt >= createdTimeBegin) && (client.CreatedAt <= createdTimeEnd))
                        {
                            users.Add(client);
                        }
                    }
                    break;

                default:
                    break;
            }
            return users;
        }

        public async Task<List<Transaction>> GetReservationStatisticsAsync(int? modeId, string userLogin, string dateRange, string fullPeriod, string reportFlag, string dateRangeReport)
        {
            var checkIn = DateTime.MinValue;
            var checkOut = DateTime.MaxValue;
            if (fullPeriod != "1" && reportFlag != "1")
            {
                checkIn = Convert.ToDateTime(dateRange.Substring(0, 10));
                checkOut = Convert.ToDateTime(dateRange.Substring(13, 10));
            }
            if (fullPeriod != "1" && reportFlag == "1")
            {
                checkIn = Convert.ToDateTime(dateRangeReport.Substring(0, 10));
                checkOut = Convert.ToDateTime(dateRangeReport.Substring(13, 10));
            }


            //var reservations = new List<Reservation>();
            var transactions = new List<Transaction>();
            switch (modeId)
            {
                case 0:
                    var allTransactionsDB = await unitOfWork.Transactions.GetAsync(
                        include: trans => trans
                        .Include(trans => trans.Services)
                        .Include(trans => trans.Client)
                        .Include(trans => trans.Reservation).ThenInclude(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel),
                        disableTracking: true);
                    foreach (var transaction in allTransactionsDB)
                    {
                        if (transaction.Services.Count() == 0)
                        {
                            if (transaction.Reservation.CreatedAt >= checkIn && transaction.Reservation.CreatedAt <= checkOut)
                            {
                                transactions.Add(transaction);
                            }
                        }
                    }
                    break;

                case 1:
                    var userDB = await unitOfWork.Clients.GetAsync(u => u.UserName == userLogin);
                    var userTransactionsDB = await unitOfWork.Transactions.GetAsync(
                        trans => trans.ClientId == userDB.FirstOrDefault().Id,
                        include: trans => trans
                        .Include(trans => trans.Services)
                        .Include(trans => trans.Client)
                        .Include(trans => trans.Reservation).ThenInclude(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel),
                        disableTracking: true);

                    foreach (var transaction in userTransactionsDB)
                    {
                        if (transaction.Services.Count() == 0)
                        {
                            if (transaction.Reservation.CreatedAt >= checkIn && transaction.Reservation.CreatedAt <= checkOut)
                            {
                                transactions.Add(transaction);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return transactions;
        }

        public async Task<List<RoomType>> GetRoomTypeStatisticsAsync(int? modeId)
        {
            var roomTypesDB = await unitOfWork.RoomTypes.GetAsync(
                include: rt => rt.Include(rt => rt.Hotel).Include(rt => rt.Rooms),
                disableTracking: true);
            return roomTypesDB.ToList();
        }
        public async Task<List<Transaction>> GetFinanceStatisticsAsync(string roleName, string dateRange, string fullPeriod, string reportFlag, string dateRangeReport)
        {
            var allClientsDB = await unitOfWork.Clients.GetAsync(
                include: c => c.Include(c => c.Transactions),
                disableTracking: true);

            var transactions = new List<Transaction>();
            var beginDate = DateTime.Today;
            var endDate = DateTime.Today;
            var beginDateReport = DateTime.Today;
            var endDateReport = DateTime.Today;

            if ((fullPeriod != "1") && (reportFlag != "1"))
            {
                beginDate = Convert.ToDateTime(dateRange.Substring(0, 10));
                endDate = Convert.ToDateTime(dateRange.Substring(13, 10));
            }
            if ((fullPeriod != "1") && (reportFlag == "1"))
            {
                beginDateReport = Convert.ToDateTime(dateRangeReport.Substring(0, 10));
                endDateReport = Convert.ToDateTime(dateRangeReport.Substring(13, 10));
                beginDate = beginDateReport;
                endDate = endDateReport;
            }

            foreach (var user in allClientsDB)
            {
                var role = await userManager.GetRolesAsync(user);
                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    if (role.Contains("client") || role.Contains("organization"))
                    {
                        if (fullPeriod == "1")
                        {
                            transactions.AddRange(user.Transactions.Where(u => u.ClientId == user.Id));
                        }
                        else
                        {
                            transactions.AddRange(user.Transactions.Where(t => t.ClientId == user.Id && t.CreatedAt >= beginDate && t.CreatedAt <= endDate));
                        }
                    }
                }
                else if (role.Contains(roleName))
                {
                    if (fullPeriod == "1")
                    {
                        transactions.AddRange(user.Transactions.Where(u => u.ClientId == user.Id));
                    }
                    else
                    {
                        transactions.AddRange(user.Transactions.Where(t => t.ClientId == user.Id && t.CreatedAt >= beginDate && t.CreatedAt <= endDate));
                    }
                }
            }
            return transactions;
        }
    }
}
