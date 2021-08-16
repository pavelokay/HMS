using HMS.Domain.UnitOfWork;
using HMS.Web.ViewModels.HotelController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Controllers
{
    public class HotelController : Controller
    {
        private IUnitOfWork unitOfWork;
        public HotelController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int hotelId)
        {
            var hotelsDB = await unitOfWork.Hotels.GetAsync(
                h => h.Id == hotelId, 
                include: h => h.Include(h => h.HotelImages), 
                disableTracking: true);
            return View(hotelsDB.FirstOrDefault());
        }

        public async Task<IActionResult> Accommodation(int hotelId)
        {
            var hotelsDB = await unitOfWork.Hotels.GetAsync(h => h.Id == hotelId, include: h => h.Include(r => r.RoomTypes).ThenInclude(i => i.RoomTypeImages));
            return View(hotelsDB.FirstOrDefault());
        }

        public async Task<IActionResult> RoomTypeDetail(int roomTypeId, int hotelId)
        {
            var roomTypeDB = await unitOfWork.RoomTypes.GetAsync(
                rt => rt.Id == roomTypeId,
                include: rt => rt.Include(rt => rt.RoomTypeImages),
                disableTracking: true);
            var hotelDB = await unitOfWork.Hotels.GetByIdAsync(hotelId);
            var roomTypeDetailsVM = new RoomTypeDetailsViewModel { RoomType = roomTypeDB.FirstOrDefault(), Hotel = hotelDB };
            return View(roomTypeDetailsVM);
        }

        public async Task<IActionResult> Contacts(int hotelId)
        {
            var hotelDB = await unitOfWork.Hotels.GetByIdAsync(hotelId);
            return View(hotelDB);
        }

        public async Task<IActionResult> Services(int hotelId)
        {
            var hotelsDB = await unitOfWork.Hotels.GetAsync(
                h => h.Id == hotelId,
                include: h => h.Include(h => h.Services),
                disableTracking: true);
            return View(hotelsDB.FirstOrDefault());
        }
    }
}
