using HMS.Domain.Models;
using HMS.Domain.UnitOfWork;
using HMS.Web.Services.Common;
using HMS.Web.ViewModels.Statistics;
using HMS.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Controllers
{
    [Authorize(Roles = "admin, accommodationOfficer, accountant")]
    public class AdminController : Controller
    {
        private IUnitOfWork unitOfWork;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        private readonly IWebHostEnvironment appEnvironment;
        private IAdminService adminService;

        public AdminController(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IWebHostEnvironment appEnvironment,
            IAdminService adminService)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appEnvironment = appEnvironment;
            this.adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Statistics()
        {
            return View();
        }
        public async Task<IActionResult> RoomStatisticsPanel()
        {
            var hotelsDB = await unitOfWork.Hotels.GetAllAsync();
            return View(hotelsDB);
        }
        public IActionResult UserStatisticsPanel()
        {
            return View();
        }
        public IActionResult ReservationStatisticsPanel()
        {
            return View();
        }

        public IActionResult PersonInfoStatisticsPanel()
        {
            return View();
        }
        public IActionResult FinanceStatisticsPanel()
        {
            return View();
        }

        public async Task<IActionResult> RoomStatistics(int? modeId, int? hotelId, string roleName, string outDay, int? minFloorNumber, int? maxFloorNumber, int? minGuest, int? maxGuest, int? roomStatusId, decimal? minRate, decimal? maxRate, string reportFlag)
        {
            var rooms = await adminService.GetRoomStatisticsAsync(modeId, hotelId, roleName, outDay, minFloorNumber, maxFloorNumber, minGuest, maxGuest, roomStatusId, minRate, maxRate);
            if (reportFlag == "1")
            {
                string filePath = Path.Combine(appEnvironment.ContentRootPath, "wwwroot/reports/ReportRooms.txt");

                string fileType = "application/txt";

                string fileName = "ReportRooms.txt";
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    await sw.WriteLineAsync("                                                      Отчет по номерам гостиничного комплекса                                                               ");
                    await sw.WriteLineAsync();
                    await sw.WriteLineAsync(" Id |||  Номер  |||   Тип номера   |||     Отель    |||  Этаж  |||    Размер    |||   Вместимость |||  Стоимость |||    Статус   ");
                    foreach (var item in rooms)
                    {
                        await sw.WriteLineAsync(item.Id.ToString().PadRight(4) + "|||" + item.Number.ToString().PadRight(8) + "|||" + item.RoomType.Name.PadRight(18) + "|||" + item.RoomType.Hotel.Name.PadRight(18) + "|||" + item.FloorNumber.ToString() + " этаж".PadRight(6) + "|||" + item.RoomType.Size.ToString() + "кв. метров".PadRight(8) + "|||" + item.RoomType.MaxGuest.ToString() + " взрослых".PadRight(8) + "|||" + item.RoomType.Rate.ToString() + "$".PadRight(8) + "|||" + ((item.RoomStatusId == 3) ? "Свободна" : "Занята"));
                    }
                }
                return PhysicalFile(filePath, fileType, fileName);
            }
            return View(new RoomStatisticsViewModel { Rooms = rooms, ModeId = modeId, HotelId = hotelId, OutDay = outDay, RoleName = roleName, MaxFloorNumber = maxFloorNumber, MinFloorNumber = minFloorNumber, MaxGuest = maxGuest, MinGuest = minGuest, MaxRate = maxRate, MinRate = minRate, RoomStatusId = roomStatusId });
        }

        public async Task<IActionResult> UserStatistics(int? modeId, int? hotelId, string userId, string roleName, int? minReservationRoomCount, string dateRange, string fullPeriod, string dateRange2, int? minFloorNumber, int? maxFloorNumber, int? minGuest, int? maxGuest, int? roomStatusId, decimal? minRate, decimal? maxRate, string reportFlag, string dateRangeReport, string dateRangeReport2, int? topPlaceCount)
        {
            var users = await adminService.GetUserStatisticsAsync(modeId, hotelId, userId, roleName, minReservationRoomCount, dateRange, fullPeriod, dateRange2, minFloorNumber, maxFloorNumber, minGuest, maxGuest, roomStatusId, minRate, maxRate, reportFlag, dateRangeReport, dateRangeReport2, topPlaceCount);
            var allRoles = roleManager.Roles.ToList();

            var userRoles = new List<IList<string>>();
            foreach(var user in users)
            {
                userRoles.Add(await userManager.GetRolesAsync(user));
            }

            if (reportFlag == "1")
            {
                string filePath = Path.Combine(appEnvironment.ContentRootPath, "wwwroot/reports/ReportUsers.txt");

                string fileType = "application/txt";

                string fileName = "ReportUsers.txt";
                var reportRoleName = (String.IsNullOrEmpty(roleName)) ? "все роли" : roleName;
                var reportPeriod = (fullPeriod == "1") ? "все время" : dateRangeReport;
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    await sw.WriteLineAsync("                                                      Отчет для роли: " + reportRoleName + "  за " + reportPeriod + "                                                               ");
                    await sw.WriteLineAsync();
                    await sw.WriteLineAsync("   Логин пользователя    |||     Имя       |||     Фамилия     |||  Пол  ||| Номер телефона |||       E-mail      |||         Адрес            |||  Дата создания  ");
                    foreach (var item in users)
                    {
                        await sw.WriteLineAsync(item.UserName.PadRight(25) + "|||" + item.FirstName.PadRight(14) + "|||" + item.LastName.PadRight(16) + "|||" + item.Gender.Name.PadRight(8) + "|||" + item.PhoneNumber.PadRight(18) + "|||" + item.Email.PadRight(25) + "|||" + item.Address.PadRight(22) + "|||" + item.CreatedAt.Date.ToString("d").PadRight(24));
                    }
                }
                return PhysicalFile(filePath, fileType, fileName);
            }
            var services = await unitOfWork.Services.GetAllAsync();
            return View(new UserStatisticsViewModel { AllRoles = allRoles, Users = users, UserRoles = userRoles, ModeId = modeId, HotelId = hotelId, RoleName = roleName, FullPeriod = fullPeriod, MinReservationRoomCount = minReservationRoomCount, TopPlaceCount = topPlaceCount, MaxFloorNumber = maxFloorNumber, MinFloorNumber = minFloorNumber, MaxGuest = maxGuest, MinGuest = minGuest, MaxRate = maxRate, MinRate = minRate, RoomStatusId = roomStatusId, UserId = userId, DateRange = dateRange, DateRange2 = dateRange2, Services = services.ToList() });
        }

        public async Task<IActionResult> ReservationStatistics(int? modeId, string userLogin, string dateRange, string fullPeriod, string reportFlag, string dateRangeReport)
        {
            var transactions = await adminService.GetReservationStatisticsAsync(modeId, userLogin, dateRange, fullPeriod, reportFlag, dateRangeReport);
            var users = new List<User>();
            var reservations = new List<Reservation>();
            foreach (var transaction in transactions)
            {
                reservations.Add(transaction.Reservation);
                users.Add(transaction.Client);
            }

            if (reportFlag == "1")
            {
                string filePath = Path.Combine(appEnvironment.ContentRootPath, "wwwroot/reports/ReportReservations.txt");

                string fileType = "application/txt";

                string fileName = "ReportReservations.txt";
                var reportPeriod = (fullPeriod == "1") ? "все время" : dateRange;
                int iterattor = 0;
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    await sw.WriteLineAsync("                                                                         Отчет за " + reportPeriod + "                                                                           ");
                    await sw.WriteLineAsync();
                    await sw.WriteLineAsync("  Id  |||          Клиент          |||     Дата создания    |||     Дата заселения    |||     Дата выселения    |||   Тип номера    |||        Отель       |||     Количество гостей    ");
                    foreach (var item in reservations)
                    {
                        await sw.WriteLineAsync(item.Id.ToString().PadRight(6) + "|||" + users[iterattor].UserName.PadRight(25) + "|||" + item.CreatedAt.Date.ToString("d").PadRight(24) + "|||" + item.CheckIn.Date.ToString("d").PadRight(24) + "|||" + item.CheckOut.Date.ToString("d").PadRight(24) + "|||" + reservations[iterattor].Rooms.FirstOrDefault().RoomType.Name.PadRight(10) + "|||" + reservations[iterattor].Rooms.FirstOrDefault().RoomType.Hotel.Name.PadRight(10) + "|||" + item.GuestCount.ToString().PadRight(10));
                        iterattor++;
                    }
                }
                return PhysicalFile(filePath, fileType, fileName);
            }
            return View(new ReservationStatisticsViewModel { Reservations = reservations, Users = users, ModeId = modeId, UserLogin = userLogin, DateRange = dateRange, FullPeriod = fullPeriod });
        }

        public async Task<IActionResult> RoomTypeStatistics(int modeId, string reportFlag)
        {
            var roomTypes = await adminService.GetRoomTypeStatisticsAsync(modeId);
            if (reportFlag == "1")
            {
                string filePath = Path.Combine(appEnvironment.ContentRootPath, "wwwroot/reports/ReportRoomTypes.txt");

                string fileType = "application/txt";

                string fileName = "ReportRoomTypes.txt";
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    await sw.WriteLineAsync("                                                                         Отчет                                                                           ");
                    await sw.WriteLineAsync();
                    await sw.WriteLineAsync("  Id  |||    Название    |||        Отель       |||  |||   Рейт    ||||||   Всего номеров  ||||||  Свободных номеров  ||||||  Занятных номеров  |||       ");
                    foreach (var item in roomTypes)
                    {
                        await sw.WriteLineAsync(item.Id.ToString().PadRight(6) + "|||" + item.Name.PadRight(12) + "|||" + item.Hotel.Name.PadRight(12) + "|||" + item.Rate.ToString().PadRight(12) + "|||" + item.Rooms.Count().ToString().PadRight(12) + "|||" + item.Rooms.Where(r => r.RoomStatusId == 3).Count().ToString().PadRight(12) + "|||" + item.Rooms.Where(r => r.RoomStatusId != 3).Count().ToString().PadRight(12));
                    }
                }
                return PhysicalFile(filePath, fileType, fileName);
            }
            return View(new RoomTypeStatisticsViewModel { ModeId = modeId, RoomTypes = roomTypes });
        }

        public async Task<IActionResult> PersonInfoStatistics(string userLogin)
        {
            var user = await userManager.FindByNameAsync(userLogin);
            var userTransactionsDB = await unitOfWork.Transactions.GetAsync(
               t => t.ClientId == user.Id,
               include: tr => tr
               .Include(trans => trans.Services)
               .Include(trans => trans.Reservation).ThenInclude(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel)
               .Include(trans => trans.TransactionStatus),
               disableTracking: true);

            var userTransactions = userTransactionsDB.ToList();
            var userReservations = new List<Reservation>();
            foreach (var transaction in userTransactions.Where(trans => trans.Services.Count == 0))
            {
                userReservations.Add(transaction.Reservation);
            }

            return View(new ProfileViewModel { User = user, Reservations = userReservations, Transactions = userTransactions });
        }

        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> FinanceStatistics(string roleName, string dateRange, string fullPeriod, string reportFlag, string dateRangeReport)
        {
            var transactions = await adminService.GetFinanceStatisticsAsync(roleName, dateRange, fullPeriod, reportFlag, dateRangeReport);

            if (reportFlag == "1")
            {
                string filePath = Path.Combine(appEnvironment.ContentRootPath, "wwwroot/reports/Report.txt");

                string fileType = "application/txt";

                string fileName = "Report.txt";
                var reportRoleName = (String.IsNullOrEmpty(roleName)) ? "все роли" : roleName;
                var reportPeriod = (fullPeriod == "1") ? "все время" : dateRange;
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    await sw.WriteLineAsync("                                                      Отчет для роли: " + reportRoleName + "  за " + reportPeriod + "                                                               ");
                    await sw.WriteLineAsync();
                    await sw.WriteLineAsync("  Id  |||      Логин пользователя      |||   Контактный телефон   |||     Дата транзакции    |||Id бронирования|||  Id платежа  |||     Стоимость    ");
                    foreach (var item in transactions)
                    {
                        await sw.WriteLineAsync(item.Id.ToString().PadRight(6) + "|||" + item.Client.UserName.PadRight(30) + "|||" + item.Client.PhoneNumber.PadRight(25) + "|||" + item.CreatedAt.Date.ToString("d").PadRight(24) + "|||" + item.ReservationId.ToString().PadRight(15) + "|||" + item.PaymentId.ToString().PadRight(12) + "|||" + Convert.ToDouble(item.Cost).ToString() + "$".PadRight(12));
                    }
                }
                return PhysicalFile(filePath, fileType, fileName);
            }
            return View(new FinanceStatisticsViewModel { Transactions = transactions, DateRange = dateRange, RoleName = roleName, FullPeriod = fullPeriod });
        }

        [HttpPost]
        [Authorize(Roles = "accountant, accommodationOfficer")]
        public async Task<IActionResult> AddService(List<int> serviceList, string userId, string userTransaction)
        {
            if (userId == null || serviceList.Count == 0)
            {
                return RedirectToAction("UserStatistics", "Admin", new { modeId = 10 });
            }
            decimal overAllCost = 0m;
            var clientBD = await unitOfWork.Clients.GetAsync(x => x.Id == userId);
            var client = clientBD.FirstOrDefault();
            string subStringId = "id:";
            int positionStringId = userTransaction.IndexOf(subStringId);
            var test = userTransaction.Substring(positionStringId + 3, userTransaction.Length - (positionStringId + 3));

            var transaction = new Transaction()
            {
                CreatedAt = DateTime.Now.Date,
                TransactionStatusId = 2,
                ClientId = client.Id,
                ReservationId = int.Parse(userTransaction.Substring(positionStringId + 3, userTransaction.Length - (positionStringId + 3)))
            };

            foreach (var serivceId in serviceList)
            {
                var service = await unitOfWork.Services.GetByIdAsync(serivceId);
                overAllCost += service.Price;
                transaction.Services.Add(service);
            }
            transaction.Cost = overAllCost;

            await unitOfWork.Transactions.AddAsync(transaction);
            return RedirectToAction("UserStatistics", "Admin", new { modeId = 10 });
        }

        [Authorize(Roles = "admin, accommodationOfficer")]
        public async Task<IActionResult> Refresh()
        {
            var reservationsDB = await unitOfWork.Reservations.GetAsync(
                include: reserv => reserv.Include(reserv => reserv.Rooms),
                disableTracking: true);
            foreach (var reservation in reservationsDB)
            {
                if (DateTime.Today.Date > reservation.CheckOut)
                {
                    foreach (var room in reservation.Rooms)
                    {
                        room.RoomStatusId = 3;
                    }
                }
                if (DateTime.Today.Date >= reservation.CheckIn && DateTime.Today.Date <= reservation.CheckOut)
                {
                    foreach (var room in reservation.Rooms)
                    {
                        room.RoomStatusId = 2;
                    }
                }
            }
            List<int> roomsId = new List<int>();
            foreach (var reservation in reservationsDB)
            {
                foreach (var room in reservation.Rooms)
                {

                    if (roomsId.FirstOrDefault(i => i == room.Id) == 0)
                    {
                        await unitOfWork.Rooms.UpdateAsync(room);
                        roomsId.Add(room.Id);
                    }
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        public async Task<string> GetUserTransaction(string userId)
        {
            var userTransactionDB = await unitOfWork.Transactions.GetAsync(x => x.ClientId == userId, 
                include: trans => trans.Include(t => t.Reservation).Include(t => t.Services),
                disableTracking: true);
            var reservationList = new List<string>();
            var userReservations = new List<Reservation>();
            string reservationInfo = "";
            //string optionTag = "";
            foreach (var transaction in userTransactionDB)
            {
                if (transaction.Services.Count() == 0)
                {
                    reservationInfo += "Бронирование от " + transaction.Reservation.CheckIn + " до " + transaction.Reservation.CheckOut + " id:" + transaction.Reservation.Id;

                    reservationList.Add(reservationInfo);
                }
                reservationInfo = "";
            }
            return JsonConvert.SerializeObject(reservationList);
        }
    }
}
