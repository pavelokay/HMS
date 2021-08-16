using HMS.Domain.Models;
using HMS.Domain.UnitOfWork;
using HMS.Web.Areas.Identity.Controllers;
using HMS.Web.Services.Common;
using HMS.Web.ViewModels.ReservationController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Controllers
{
    public class ReservationController : Controller
    {
        private IUnitOfWork unitOfWork;
        private IReservationService reservationService;
        public ReservationController(
            IUnitOfWork unitOfWork,
            IReservationService reservationService)
        {
            this.unitOfWork = unitOfWork;
            this.reservationService = reservationService;
        }

        public async Task<IActionResult> Index(int? hotelId)
        {
            var hotelsDB = await unitOfWork.Hotels.GetAllAsync();
            if (hotelId != null)
            {
                ViewData["HotelId"] = hotelId;
            }
            return View(hotelsDB);
        }
        public async Task<IActionResult> ChooseRoom(int hotelId, string dateRange, int? guestCount = 1, int? roomCount = 1)
        {
            var checkIn = Convert.ToDateTime(dateRange.Substring(0, 10));
            var checkOut = Convert.ToDateTime(dateRange.Substring(13, 10));
            var roomTypesDB = await reservationService.FindRoomTypesAsync(hotelId, checkIn, checkOut, guestCount, roomCount);
            var hotelDB = await unitOfWork.Hotels.GetByIdAsync(hotelId);
            var ChooseRoomVM = new ChooseRoomViewModel() { RoomTypes = roomTypesDB, Hotel = hotelDB, CheckIn = checkIn, CheckOut = checkOut, GuestCount = guestCount, RoomCount = roomCount };
            return View(ChooseRoomVM);
        }

        [Authorize]
        public async Task<IActionResult> AccommodationPayment(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int guestCount, int roomCount)
        {
            var hotelDB = await unitOfWork.Hotels.GetByIdAsync(hotelId);
            var roomTypeDB = await unitOfWork.RoomTypes.GetByIdAsync(roomTypeId);
            decimal price = roomTypeDB.Rate * Convert.ToDecimal(checkOut.Subtract(checkIn).Days) * roomCount;
            int? sale = null;
            decimal? salePrice = null;
            if (User.IsInRole("organization"))
            {
                if (roomCount > 1 && roomCount <= 3)
                {
                    salePrice = price * 0.97m;
                    sale = 3;
                }
                if (roomCount > 3 && roomCount <= 6)
                {
                    salePrice = price * 0.94m;
                    sale = 6;
                }
                if (roomCount > 6 && roomCount <= 10)
                {
                    salePrice = price * 0.90m;
                    sale = 10;
                }
                else
                {
                    if (roomCount > 10)
                    {
                        salePrice = price * 0.85m;
                        sale = 15;
                    }
                }
            }
            var accommodationPaymentVM = new AccommodationPaymentViewModel { Hotel = hotelDB, RoomType = roomTypeDB, CheckIn = checkIn, CheckOut = checkOut, GuestCount = guestCount, RoomCount = roomCount, Price = price, SalePrice = salePrice, Sale = sale };
            return View(accommodationPaymentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccommodationPayment(int roomTypeId, DateTime checkIn, DateTime checkOut, int guestCount, int roomCount, decimal cost)
        {
            var reservation = await reservationService.MakeReservationAsync(roomTypeId, checkIn, checkOut, guestCount, roomCount);
            if (reservation == null)
            {
                return BadRequest();
            }

            var payment = new Payment()
            {
                PaymentTypeId = 2
            };
            await unitOfWork.Payments.AddAsync(payment);
            await unitOfWork.SaveAsync();

            var userName = User.Identity.Name;
            var clientsDB = await unitOfWork.Clients.GetAsync(u => u.UserName == userName);
            var transaction = new Transaction()
            {
                ClientId = clientsDB.FirstOrDefault().Id,
                ReservationId = reservation.Id,
                PaymentId = payment.Id,
                TransactionStatusId = 1,
                CreatedAt = DateTime.Now.Date,
                Cost = cost
            };
            await unitOfWork.Transactions.AddAsync(transaction);
            await unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServicePayment(string transactionId, string cardOwner, string cardNumber)
        {
            if (String.IsNullOrWhiteSpace(transactionId))
            {
                return BadRequest();
            }
            var transactionDB = await unitOfWork.Transactions.GetByIdAsync(int.Parse(transactionId));
            if (transactionDB == null)
            {
                return NotFound();
            }

            var payment = new Payment()
            {
                PaymentTypeId = 2,
            };
            await unitOfWork.Payments.AddAsync(payment);
            await unitOfWork.SaveAsync();

            transactionDB.PaymentId = payment.Id;
            transactionDB.TransactionStatusId = 1;
            await unitOfWork.Transactions.UpdateAsync(transactionDB);
            await unitOfWork.SaveAsync();

            return Content(payment.Id.ToString());
        }

        public async Task<IActionResult> CancelReservation(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var reservationDB = await unitOfWork.Reservations.GetAsync(reserv => reserv.Id == id,
                include: reserv => reserv
                .Include(reserv => reserv.Transactions).ThenInclude(trans => trans.Services)
                .Include(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel),
                disableTracking: true);

            var reservation = reservationDB.FirstOrDefault();
            if (reservation == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return View(reservation);
        }

        [HttpPost, ActionName("CancelReservation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservationPost(int id)
        {
            var reservationDB = await unitOfWork.Reservations.GetByIdAsync((int)id);
            if (reservationDB == null)
            {
                return RedirectToAction(nameof(AccountController.Profile));
            }
            try
            {
                await unitOfWork.Reservations.DeleteAsync(reservationDB);
                await unitOfWork.SaveAsync();
                return RedirectToAction(nameof(AccountController.Profile));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(CancelReservation), new { id = id, saveChangesError = true });
            }


        }
    }
}
