using HMS.Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;
        public HomeController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            //var hotelComplexesDB = await unitOfWork.HotelComplexes.GetAsync(includeString: "HotelComplexImages", disableTracking: true);
            var hotelComplexesDB = await unitOfWork.HotelComplexes.GetAsync(
                include: complex => complex.Include(complex => complex.HotelComplexImages),
                disableTracking: true);

            return View(hotelComplexesDB.FirstOrDefault());
        }

        public async Task<IActionResult> FindHotel()
        {
            var hotelsDB = await unitOfWork.Hotels.GetAsync(
                include: h => h.Include(h => h.HotelImages),
                disableTracking: true);
            return View(hotelsDB);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
