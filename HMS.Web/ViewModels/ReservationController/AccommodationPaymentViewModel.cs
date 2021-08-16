using HMS.Domain.Models;
using System;


namespace HMS.Web.ViewModels.ReservationController
{
    public class AccommodationPaymentViewModel
    {
        public Hotel Hotel { get; set; }
        public RoomType RoomType { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int RoomCount { get; set; }
        public int GuestCount { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Sale { get; set; }
    }
}
